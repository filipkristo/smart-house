using Newtonsoft.Json;

namespace SmartHouseLambda.Model
{
    public class RequestEndpoint
    {
        /// <summary>
        /// A device identifier. The identifier must be unique across all devices owned by an end user within the domain for the skill. In addition, the identifier needs to be consistent across multiple discovery requests for the same device. An identifier can contain letters or numbers, spaces, and the following special characters: _ - = # ; : ? @ &. The identifier cannot exceed 256 characters.
        /// </summary>
        [JsonProperty("endpointId")]
        public string EndpointId { get; set; }

        [JsonProperty("scope")]
        public Scope Scope { get; set; }

        /// <summary>
        /// String name/value pairs that provide additional information about a device for use by the skill. The contents of this property cannot exceed 5000 bytes. The API doesn't use or understand this data.
        /// </summary>
        [JsonProperty("cookie")]
        public Cookie Cookie { get; set; }
    }
}