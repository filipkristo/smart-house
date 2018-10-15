using Newtonsoft.Json;

namespace SmartHouseLambda.Model
{
    public class Header
    {

        [JsonProperty("messageId")]
        public string MessageId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("namespace")]
        public string Namespace { get; set; }

        [JsonProperty("payloadVersion")]
        public string PayloadVersion { get; set; }



        // Not needed for discovery.
        // Needed for StateReport
        [JsonProperty("correlationToken")]
        public string CorrelationToken { get; set; }
    }
}