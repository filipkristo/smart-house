using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHouse.Lib
{
    public class SunriseSunsetResult
    {
        [JsonProperty("results")]
        public SunriseSunsetData Results { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }
}
