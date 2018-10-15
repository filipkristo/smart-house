using Newtonsoft.Json;

namespace SmartHouseLambda.Model
{
    public class CookTimeControlResponse
    {
        [JsonProperty("context")]
        public Context Context { get; set; }

        [JsonProperty("event")]
        public Event Event { get; set; }
    }
}