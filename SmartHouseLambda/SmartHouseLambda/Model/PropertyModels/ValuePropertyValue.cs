using Newtonsoft.Json;

namespace SmartHouseLambda.Model.PropertyModels
{
    public class ValuePropertyValue
    {
        [JsonProperty("value")]
        public string Value { get; set; }
    }
}