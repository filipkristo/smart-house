using System.Collections.Generic;
using Newtonsoft.Json;
using SmartHouseLambda.Model.PropertyModels;

namespace SmartHouseLambda.Model
{
    public class Context
    {
        [JsonProperty("properties")]
        public List<Property> Properties { get; set; }
    }
}