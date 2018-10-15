using System;
using Newtonsoft.Json;

namespace SmartHouseLambda.Model.PropertyModels
{
    public class DateTimeValueProperty : Property
    {
        [JsonProperty("value")]
        public DateTime Value { get; set; }
    }
}