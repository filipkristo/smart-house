using Newtonsoft.Json;

namespace SmartHouseLambda.Model
{
    public class ErrorResponse
    {
        [JsonProperty("event")]
        public Event Event { get; set; }
    }
}