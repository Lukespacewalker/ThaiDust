using System.Collections.Generic;
using Windows.Web.Http;

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
}