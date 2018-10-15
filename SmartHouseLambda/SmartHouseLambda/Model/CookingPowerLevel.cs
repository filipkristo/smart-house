using Newtonsoft.Json;

namespace SmartHouseLambda.Model
{
    public class CookingPowerLevel
    {
        [JsonProperty("@type")]
        public string PowerLevel { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }
}