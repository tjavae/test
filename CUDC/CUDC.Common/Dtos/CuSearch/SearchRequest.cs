using Newtonsoft.Json;

namespace CUDC.Common.Dtos.CuSearch
{
    public class SearchRequest
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("region")]
        public string Region { get; set; }

        [JsonProperty("se")]
        public string SE { get; set; }

        [JsonProperty("district")]
        public string District { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("status")]
        public static string Status => "A";

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }
    }
}
