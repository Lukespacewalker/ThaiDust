using System.Collections.Generic;
using System.Net.Http;

namespace ThaiDust.Core.Dto
{
    public class GetParamListDto
    {
        public string StationId { get; set; }
        public string Action { get; set; } = "getParamList";

        public FormUrlEncodedContent GenerateFormUrlEncodedContent()
        {
            return new FormUrlEncodedContent(
                new[]
                {
                    new KeyValuePair<string, string>("stationId",StationId),
                    new KeyValuePair<string, string>("action",Action)
                }
            );
        }
    }
}