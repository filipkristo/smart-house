using Newtonsoft.Json;

namespace SmartHouseLambda.Model
{
    public class Directive
    {
        [JsonProperty("header")]
        public Header Header { get; set; }

        [JsonProperty("payload")]
        public Payload Payload { get; set; }

        /// <summary>
        /// ReportState uses endpoint rather than payload.
        /// </summary>
        [JsonProperty("endpoint")]
        public RequestEndpoint Endpoint { get; set; }
    }
}