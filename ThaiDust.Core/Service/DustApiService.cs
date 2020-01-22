using System;
using System.Net.Http;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using Splat;
using ThaiDust.Core.Dto;
using ThaiDust.Core.Model.Persistent;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using ThaiDust.Core.Model;

namespace ThaiDust.Core.Service
{
    public class DustService
    {
        private DustDataService _dataService;
        private DustApiService _apiService;

        public DustService()
        {
            _dataService = Locator.Current.GetService<DustDataService>();
            _apiService = Locator.Current.GetService<DustApiService>();
        }

        public IObservable<IEnumerable<StationParam>> GetAvailableParameters(Station station)
        {
            return _apiService.GetStationAvailableParameters(station);
        }

        public IObservable<IEnumerable<Record>> GetStationData(string stationCode, string parameter)
        {
            // Get Data From Database first
            var station = _dataService.LoadDustData(stationCode);
            var subject = new Subject<IEnumerable<Record>>();
            var startDate = new DateTime(2018,10,1,0,0,0);
            if (station != null)
            {
                var data = station.Records.Where(r => r.Param == parameter).OrderBy(r => r.DateTime);
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


    internal class DustApiService
    {
        private readonly HttpClient _client;

        public DustApiService(HttpClient httpClient = null)
        {
            this._client = httpClient ?? Locator.Current.GetService<HttpClient>();
        }

        public IObservable<IEnumerable<StationParam>> GetStationAvailableParameters(Station station)
        {
            var dto = new GetParamListDto { StationId = station.Code };
            return _client.PostAsync(new Uri("http://aqmthai.com/includes/getManReport.php"),
                    dto.GenerateFormUrlEncodedContent())
                .ToObservable()
                .Select(async r => await r.Content.ReadAsStringAsync())
                .Switch().Select(XmlParser.ParseParameter);
        }

        public IObservable<IEnumerable<Record>> GetStationData(DateTime startDate, DateTime endDate, string stationCode, string parameter)
        {
            //var startDate = new DateTime(StartDate.Value.Year, StartDate.Value.Month, StartDate.Value.Day, StartTime.Value.Hours, StartTime.Value.Minutes, StartTime.Value.Seconds);
            //var endDate = new DateTime(EndDate.Value.Year, EndDate.Value.Month, EndDate.Value.Day, EndTime.Value.Hours, EndTime.Value.Minutes, EndTime.Value.Seconds);
            var dto = new GetDataDto { StationId = stationCode, ParamValue = parameter, StartDate = startDate, EndDate = endDate };
            var payload = dto.GenerateFormUrlEncodedContent();
            return _client.PostAsync(new Uri("http://aqmthai.com/includes/getMultiManReport.php"),
                    payload)
                .ToObservable()
                .Select(async r => await r.Content.ReadAsStringAsync())
                .Switch().Select(XmlParser.ParseData).Do(records =>
                {
                    foreach (var record in records)
                    {
                        record.Param = parameter;
                    }
                });

        }
    }
}