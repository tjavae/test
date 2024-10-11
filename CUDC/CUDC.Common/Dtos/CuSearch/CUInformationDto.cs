using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace CUDC.Common.Dtos.CuSearch
{
    public class CUInformationDto
    {
        [JsonProperty("cuNumber")]
        public int CuNumber { get; set; }
        [JsonProperty("joinNumber")]
        public int JoinNumber { get; set; }
        [JsonProperty("cuActualState")]
        public string ActualState { get; set; }
        [JsonProperty("cuType")]
        public string CuType { get; set; }
        [JsonProperty("region")]
        public string Region { get; set; }
        [JsonProperty("se")]
        public string SE { get; set; }
        [JsonProperty("countyCode")]
        public int CountyCode { get; set; }
        [JsonProperty("district")]
        public int District { get; set; }
    }
}
