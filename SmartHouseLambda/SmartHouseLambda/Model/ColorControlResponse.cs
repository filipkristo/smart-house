using Newtonsoft.Json;

namespace SmartHouseLambda.Model
{
    public class ColorControlResponse
    {
        [JsonProperty("context")]
        public Context Context { get; set; }

        [JsonProperty("event")]
        public Event Event { get; set; }
    }
}