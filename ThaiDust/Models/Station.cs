using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using Windows.Web.Http;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace ThaiDust.Models
{
    public class Station : ReactiveObject
    {
        [Reactive] public string Id { get; set; }
        [Reactive] public string Name { get; set; }
    }

    public class StationParam : ReactiveObject
    {
        [Reactive] public string Param { get; set; }
        [Reactive] public string Name { get; set; }
    }
}

namespace ThaiDust.Dto
{
    public class GetParamListDto
    {
        public string StationId { get; set; }
        public string Action { get; set; } = "getParamList";

        public HttpFormUrlEncodedContent GenerateFormUrlEncodedContent()
        {
            return new HttpFormUrlEncodedContent(
                new[]
                {
                    new KeyValuePair<string, string>("stationId",StationId),
                    new KeyValuePair<string, string>("action",Action)
                }
            );
        }
    }

    public class GetDataDto
    {
        public string StationId { get; set; }
        public string Action { get; set; } = "showTable";
        public string ParamValue { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string ReportType { get; set; } = "Raw";
        public string DataReportType { get; set; } = "_h";
        public int ShowNumRow { get; set; } = 10000;
        public int PageNo { get; set; } = 1;

        public HttpFormUrlEncodedContent GenerateFormUrlEncodedContent()
        {
            return new HttpFormUrlEncodedContent(
                new[]
                {
                    new KeyValuePair<string, string>("stationId",StationId),
                    new KeyValuePair<string, string>("action",Action),
                    new KeyValuePair<string, string>("paramValue",ParamValue),
                    new KeyValuePair<string, string>("startDate",StartDate.ToString("yyyy-MM-dd", CultureInfo.GetCultureInfo("en-US"))),
                    new KeyValuePair<string, string>("startTime",StartDate.ToString("hh:mm:ss", CultureInfo.GetCultureInfo("en-US"))),
                    new KeyValuePair<string, string>("endDate",EndDate.ToString("yyyy-MM-dd", CultureInfo.GetCultureInfo("en-US"))),
                    new KeyValuePair<string, string>("endTime",EndDate.ToString("hh:mm:ss", CultureInfo.GetCultureInfo("en-US"))),
                    new KeyValuePair<string, string>("reportType",ReportType),
                    new KeyValuePair<string, string>("dataReportType",DataReportType),
                    new KeyValuePair<string, string>("showNumRow",ShowNumRow.ToString()),
                    new KeyValuePair<string, string>("pageNo",PageNo.ToString())
                }
            );
        }
    }
}
