using System.Collections.Generic;
using Newtonsoft.Json;

namespace SmartHouseLambda.Model
{
    public class EventPayload
    {
        [JsonProperty("endpoints")]
        public IEnumerable<Endpoint> Endpoints { get; set; }

        /// <summary>
        /// Used for error messages (e.g. ENDPOINT_UNREACHABLE)
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }

        /// <summary>
        /// Used for error messages
        /// </summary>
        [JsonProperty("message")]
        public string Message { get; set; }
    }
}