using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Subjects;
using DynamicData;
using DynamicData.Binding;
using Splat;
using ThaiDust.Core.Model;
using ThaiDust.Core.Model.Persistent;

namespace ThaiDust.Core.Service
{
    public class DustService
    {
        private readonly DustDataService _dataService;
        private readonly ThaiPollutionControlDataAPI _api;

        private readonly SourceList<Station> _managedStationsSource = new SourceList<Station>();

        private readonly ReadOnlyObservableCollection<Station> _managedStations;
        public ReadOnlyObservableCollection<Station> ManagedStations => _managedStations;

        public ObservableCollectionExtended<Station> ManagedStations2 { get; } = new ObservableCollectionExtended<Station>();

        public DustService(DustDataService dustDataService = null, ThaiPollutionControlDataAPI thaiPollutionControlDataApi = null)
        {
            _dataService = dustDataService ?? Locator.Current.GetService<DustDataService>();
            _api = thaiPollutionControlDataApi ?? Locator.Current.GetService<ThaiPollutionControlDataAPI>();

            // Setup source output
            _managedStationsSource.Connect().Bind(out _managedStations).Subscribe();
            _managedStationsSource.Connect().Bind(ManagedStations2).Subscribe();
            // Load all station from local database
            _managedStationsSource.AddRange(_dataService.GetAllStations());
        }

        public void SaveManagedStationsToDatabase()
        {
            Station[] managed = ManagedStations2.ToArray();
            Station[] diff = _dataService.GetAllStations().Except(managed).ToArray();
            _dataService.RemoveStations(diff);
            _dataService.AddOrUpdateStations(managed);
            _dataService.Commit();
        }

        //public void AddStations(params Station[] stations)
        //{
        //    _managedStationsSource.AddRange(stations);
        //}

        //public void RemoveStations(params Station[] stations)
        //{
        //    _managedStationsSource.RemoveMany(stations);
        //}

        public IObservable<IList<StationParam>> GetAvailableParameters(Station station)
        {
            return _api.GetStationAvailableParameters(station);
        }

        public IObservable<Record[]> GetStationData(string stationCode, RecordType parameter)
        {
            // Get Data From Database first
            Station databaseStation = _dataService.GetStation(stationCode);
            Record[] data = databaseStation?.Records.Where(r => r.Type == parameter).OrderBy(r => r.DateTime).ToArray();
            var subject = new Subject<Record[]>();
            var startDate = new DateTime(2019, 10, 1, 0, 0, 0);
            if (data != null)
            {
                if (data.Any()) startDate = data.Last().DateTime.AddHours(1);
                // Sent the old data first
                subject.OnNext(data);
            }
            // Get Data from API
            _api.GetStationData(startDate, DateTime.Now, stationCode, parameter)
                .Subscribe(async records =>
                {
                    // Save data to database
                    databaseStation.Records.Add(records);
                    await _dataService.Commit();
                    // Sent Data
                    subject.OnNext(data == null ? records : data.Union(records).ToArray());
                    //_dataService.AddOrUpdateRecords(stationCode, records);
                    subject.OnCompleted();
                });
            return subject;
        }
    }
}