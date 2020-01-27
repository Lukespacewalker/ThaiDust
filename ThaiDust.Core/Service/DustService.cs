using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using Splat;
using ThaiDust.Core.Model;
using ThaiDust.Core.Model.Persistent;

namespace ThaiDust.Core.Service
{
    public class DustService
    {
        private readonly DustDataService _dataService;
        private readonly DustApiService _apiService;

        public DustService(DustDataService dustDataService = null, DustApiService dustApiService = null)
        {
            _dataService = dustDataService ?? Locator.Current.GetService<DustDataService>();
            _apiService = dustApiService ?? Locator.Current.GetService<DustApiService>();
        }

        public IObservable<IEnumerable<StationParam>> GetAvailableParameters(Station station)
        {
            return _apiService.GetStationAvailableParameters(station);
        }

        public IObservable<IEnumerable<Record>> GetStationData(string stationCode, RecordType parameter)
        {
            // Get Data From Database first
            var station = _dataService.LoadDustData(stationCode);
            var subject = new Subject<IEnumerable<Record>>();
            var startDate = new DateTime(2018,10,1,0,0,0);
            if (station != null)
            {
                var data = station.Records.Where(r => r.Type == parameter).OrderBy(r => r.DateTime);
                startDate = data.Last().DateTime;
                subject.OnNext(data);
            }
            // Sent the old data first
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