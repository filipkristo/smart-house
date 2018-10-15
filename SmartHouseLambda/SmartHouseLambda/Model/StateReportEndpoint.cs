using Newtonsoft.Json;

namespace SmartHouseLambda.Model
{
    public class StateReportEndpoint
    {
        [JsonProperty("scope")]
        public Scope Scope { get; set; }

        [JsonProperty("endpointId")]
        public string EndpointId { get; set; }
    }
}