using Newtonsoft.Json;

namespace SmartHouseLambda.Model
{
    public class Event
    {
        [JsonProperty("header")]
        public Header Header { get; set; }

        /// <summary>
        /// Used for Device Discover.
        /// </summary>
        [JsonProperty("payload")]
        public EventPayload Payload { get; set; }

        /// <summary>
        /// Used for State Report
        /// </summary>
        /// <see cref="https://github.com/alexa/alexa-smarthome/blob/master/sample_messages/StateReport/StateReport.json"/>
        [JsonProperty("endpoint")]
        public StateReportEndpoint Endpoint { get; set; }
    }
}