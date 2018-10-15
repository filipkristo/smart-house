using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartHouseLambda.Model
{
    public class Input
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
