using Newtonsoft.Json;

namespace SmartHouseLambda.Model
{
    public class TemperaturePropertyValue
    {
        [JsonProperty("value")]
        public float Value { get; set; }

        [JsonProperty("scale")]
        public string Scale { get; set; }
    }
}