using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
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
        private readonly ThaiPollutionControlDataApi _api;

        private readonly SourceList<Station> _managedStationsSource = new SourceList<Station>();

        public ObservableCollectionExtended<Station> ManagedStations { get; } = new ObservableCollectionExtended<Station>();

        public DustService(DustDataService dustDataService = null, ThaiPollutionControlDataApi thaiPollutionControlDataApi = null)
        {
            _dataService = dustDataService ?? Locator.Current.GetService<DustDataService>();
            _api = thaiPollutionControlDataApi ?? Locator.Current.GetService<ThaiPollutionControlDataApi>();

            // Setup source output
            _managedStationsSource.Connect().Bind(ManagedStations).Subscribe();
            // Load all station from local database
            _managedStationsSource.AddRange(_dataService.GetAllStations());
        }

        public async Task SaveManagedStationsToDatabase()
        {
            Station[] managed = ManagedStations.ToArray();
            Station[] diff = _dataService.GetAllStations().Except(managed).ToArray();
            _dataService.RemoveStations(diff);
            await _dataService.AddOrUpdateStations(managed);
            await _dataService.CommitAsync();
        }

        public IObservable<IList<StationParam>> GetAvailableParametersAsync(Station station)
        {
            return _api.GetStationAvailableParametersAsync(station);
        }

        public IObservable<Record[]> GetStationRecordsAsync(string stationCode, params RecordType[] parameters)
        {
            // Initial data
            IObservable<Record[]> initialDataObservable = _dataService.GetStationAsync(stationCode)
                .Select(station =>
                {
                    return station.Records.OrderBy(r => r.DateTime).ToArray();
                }).Publish().RefCount();
            IObservable<Record[]> subsequentDataObservable = initialDataObservable.Select(initialRecords =>
            {
                var initialDate = new DateTime(2019, 10, 1, 0, 0, 0);
                return parameters.Select(parameter =>
                {
                    var lastRecord = initialRecords.LastOrDefault(s => s.Type == parameter);
                    if (lastRecord != null) return (parameter, lastRecord.DateTime.AddHours(1));
                    return (parameter, initialDate);
                }).ToObservable().Select(p =>
                {
                    var (parameter, startDate) = p;
                    return _api.GetStationData(startDate, DateTime.Now, stationCode, parameter);
                }).Merge(2);
            }).Switch().Do(async records =>
            {
                // Save data to database
                var databaseStation = await _dataService.GetStationAsync(stationCode);
                databaseStation?.Records.Add(records);
                await _dataService.CommitAsync();
            });

            return subsequentDataObservable;

            // Get Data From Database first
            //return _dataService.GetStationAsync(stationCode)
            //    .Select(databaseStation =>
            //{
            //    // BehaviorSubject for emitting results
            //    var subject = new Subject<Record[]>();
            //    // Emit all exiting records
            //    Record[] stationRecords = databaseStation.Records.OrderBy(r => r.DateTime).ToArray();
            //    subject.OnNext(stationRecords);

            //    var initialDate = new DateTime(2019, 10, 1, 0, 0, 0);
            //    IObservable<Record[]> b = parameters.Select(parameter =>
            //    {
            //        var lastRecord = stationRecords.LastOrDefault(s => s.Type == parameter);
            //        if (lastRecord != null) return (parameter, lastRecord.DateTime.AddHours(1));
            //        return (parameter, initialDate);
            //    }).ToObservable().Select(p =>
            //    {
            //        var (parameter, startDate) = p;
            //        return _api.GetStationData(startDate, DateTime.Now, stationCode, parameter);
            //    }).Merge(2);

            //    return subject;
            //}).Switch();

            /*
                      // Get Data from API
                _api.GetStationData(initialDate, DateTime.Now, stationCode, parameter)
                    .Subscribe(async records =>
                    {
                        // Save data to database
                        databaseStation?.Records.Add(records);
                        await _dataService.CommitAsync();
                        // Sent Data
                        subject.OnNext(stationRecords == null ? records : stationRecords.Union(records).ToArray());
                        //_dataService.AddOrUpdateRecords(stationCode, records);
                        subject.OnCompleted();
                    });
             */
        }
    }
}