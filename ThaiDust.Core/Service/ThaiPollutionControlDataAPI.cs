using System;
using System.Net.Http;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using Splat;
using ThaiDust.Core.Dto;
using ThaiDust.Core.Model.Persistent;
using System.Collections.Generic;
using System.Linq;
using ThaiDust.Core.Model;

namespace ThaiDust.Core.Service
{
    public class ThaiPollutionControlDataAPI
    {
        private readonly HttpClient _client;

        public ThaiPollutionControlDataAPI(HttpClient httpClient = null)
        {
            this._client = httpClient ?? Locator.Current.GetService<HttpClient>();
        }

        public IObservable<IList<StationParam>> GetStationAvailableParameters(Station station)
        {
            var dto = new GetParamListDto { StationId = station.Code };
            return _client.PostAsync(new Uri("http://aqmthai.com/includes/getManReport.php"),
                    dto.GenerateFormUrlEncodedContent())
                .ToObservable()
                .Select(async r => await r.Content.ReadAsStringAsync())
                .Switch().Select(XmlParser.ParseParameter);
        }

        public IObservable<Record[]> GetStationData(DateTime startDate, DateTime endDate, string stationCode, RecordType parameter)
        {
            // Convert Enum to Text
            string param = parameter switch
            {
                RecordType.PM25 => "PM2.5",
                _ => Enum.GetName(typeof(RecordType), parameter)
            };
            //var startDate = new DateTime(StartDate.Value.Year, StartDate.Value.Month, StartDate.Value.Day, StartTime.Value.Hours, StartTime.Value.Minutes, StartTime.Value.Seconds);
            //var endDate = new DateTime(EndDate.Value.Year, EndDate.Value.Month, EndDate.Value.Day, EndTime.Value.Hours, EndTime.Value.Minutes, EndTime.Value.Seconds);
            var dto = new GetDataDto { StationId = stationCode, ParamValue = param, StartDate = startDate, EndDate = endDate };
            var payload = dto.GenerateFormUrlEncodedContent();
            return _client.PostAsync(new Uri("http://aqmthai.com/includes/getMultiManReport.php"),
                    payload)
                .ToObservable()
                .Select(async r => await r.Content.ReadAsStringAsync())
                .Switch().Select(XmlParser.ParseData).Do(records =>
                {
                    foreach (var record in records)
                    {
                        record.Type = parameter;
                    }
                });

        }
    }
}