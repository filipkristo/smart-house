using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartHouseLambda.Model
{
    public abstract class BaseResponse
    {
        [JsonProperty("event")]
        public Event Event { get; set; }
    }
}
