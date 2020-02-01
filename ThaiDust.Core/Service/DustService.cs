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
        private readonly DustApiService _apiService;

        private readonly SourceList<Station> _managedStationsSource = new SourceList<Station>();

        private readonly ReadOnlyObservableCollection<Station> _managedStations;
        public ReadOnlyObservableCollection<Station> ManagedStations => _managedStations;

        public ObservableCollectionExtended<Station> ManangedStations2 { get; } = new ObservableCollectionExtended<Station>();

        public DustService(DustDataService dustDataService = null, DustApiService dustApiService = null)
        {
            _dataService = dustDataService ?? Locator.Current.GetService<DustDataService>();
            _apiService = dustApiService ?? Locator.Current.GetService<DustApiService>();

            // Setup source output
            _managedStationsSource.Connect().Bind(out _managedStations).Subscribe();
            _managedStationsSource.Connect().Bind(ManangedStations2).Subscribe();
            // Load all station from local database
            _managedStationsSource.AddRange(_dataService.GetAllStations());
        }

        public void SaveManagedStationsToDatabase()
        {
            Station[] temp = _dataService.GetAllStations().Intersect(ManangedStations2).ToArray();
            _dataService.AddOrUpdateStations();
        }

        public void AddStations(params Station[] stations)
        {
            _managedStationsSource.AddRange(stations);
        }

        public void RemoveStations(params Station[] stations)
        {
            _managedStationsSource.RemoveMany(stations);
        }

        public IObservable<IEnumerable<StationParam>> GetAvailableParameters(Station station)
        {
            return _apiService.GetStationAvailableParameters(station);
        }

        public IObservable<IEnumerable<Record>> GetStationData(string stationCode, RecordType parameter)
        { 
            // Get Data From Database first
            var databaseStation = _dataService.GetStation(stationCode);
            var subject = new Subject<IEnumerable<Record>>();
            var startDate = new DateTime(2018, 10, 1, 0, 0, 0);
            if (databaseStation != null)
            {
                var data = databaseStation.Records.Where(r => r.Type == parameter).OrderBy(r => r.DateTime);
                startDate = data.Last().DateTime;
                // Sent the old data first
                subject.OnNext(data);
            }
            // Get Data from API
            _apiService.GetStationData(startDate, DateTime.Now, stationCode, parameter)
                .Subscribe(records =>
                {
                    subject.OnNext(records);
                    // Save data to database
                    _dataService.InsertOrUpdateDustData(stationCode, records);
                    subject.OnCompleted();
                });
            return subject;
        }
    }
}