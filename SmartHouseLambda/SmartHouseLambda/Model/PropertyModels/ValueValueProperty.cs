using Newtonsoft.Json;

namespace SmartHouseLambda.Model.PropertyModels
{
    public class ValueValueProperty : Property
    {
        [JsonProperty("value")]
        public ValuePropertyValue Value { get; set; }
    }
}