using Newtonsoft.Json;

namespace SmartHouseLambda.Model
{
    public class Scope
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }
    }
}