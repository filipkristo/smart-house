using Newtonsoft.Json;

namespace SmartHouseLambda.Model.PropertyModels
{
    public class BooleanValueProperty : Property
    {
        [JsonProperty("value")]
        public bool Value { get; set; }
    }
}