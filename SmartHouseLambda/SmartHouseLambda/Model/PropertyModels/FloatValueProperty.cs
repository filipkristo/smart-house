using Newtonsoft.Json;

namespace SmartHouseLambda.Model.PropertyModels
{
    public class FloatValueProperty : Property
    {
        [JsonProperty("value")]
        public float Value { get; set; }
    }
}