using Newtonsoft.Json;

namespace SmartHouseLambda.Model
{
    /// <summary>
    /// String name/value pairs that provide additional information about a device for use by the skill. The contents of this property cannot exceed 5000 bytes. The API doesn't use or understand this data.
    /// </summary>
    public class Cookie
    {
        /// <summary>
        /// The devices @Username in Tinamous
        /// </summary>
        [JsonProperty("username")]
        public string Username { get; set; }

        //[JsonProperty("portNumber")]
        //public int? PortNumber { get; set; }

        /// <summary>
        /// The device id (removed from endpointId)
        /// </summary>
        [JsonProperty("deviceId")]
        public string DeviceId { get; set; }

        // TODO: Add port?, actual deviceId?, Display name, other???
    }
}