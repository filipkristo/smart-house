using System.Collections.Generic;
using Newtonsoft.Json;

namespace SmartHouseLambda.Model
{
    public class Endpoint
    {
        /// <summary>
        /// A device identifier. The identifier must be unique across all devices owned by an end user within the domain for the skill. In addition, the identifier needs to be consistent across multiple discovery requests for the same device. An identifier can contain letters or numbers, spaces, and the following special characters: _ - = # ; : ? @ &. The identifier cannot exceed 256 characters.
        /// </summary>
        [JsonProperty("endpointId")]
        public string EndpointId { get; set; }

        /// <summary>
        /// The name used by the customer to identify the device. This value cannot exceed 128 characters and should not contain special characters or punctuation.	
        /// </summary>
        [JsonProperty("friendlyName")]
        public string FriendlyName { get; set; }

        /// <summary>
        /// A human-readable description of the device. This value cannot exceed 128 characters. The description should contain the manufacturer name or how the device is connected. For example, "Smart Lock by Sample Manufacturer" or "WiFi Thermostat connected via SmartHub". This value cannot exceed 128 characters.
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// The name of the device manufacturer. This value cannot exceed 128 characters.	
        /// </summary>
        [JsonProperty("manufacturerName")]
        public string ManufacturerName { get; set; }

        /// <summary>
        /// Indicates the group name where the device should display in the Alexa app. See Display categories for supported values.
        /// </summary>
        /// <see cref="https://developer.amazon.com/docs/device-apis/alexa-discovery.html#display-categories"/>
        [JsonProperty("displayCategories")]
        public string[] DisplayCategories { get; set; }

        /// <summary>
        /// String name/value pairs that provide additional information about a device for use by the skill. The contents of this property cannot exceed 5000 bytes. The API doesn't use or understand this data.
        /// </summary>
        [JsonProperty("cookie")]
        public Cookie Cookie { get; set; }

        /// <summary>
        /// An array of capability objects that represents actions particular device supports and can respond to. A capability object can contain different fields depending on the type.
        /// </summary>
        /// <see cref="https://developer.amazon.com/docs/device-apis/alexa-discovery.html#capability-object"/>
        [JsonProperty("capabilities")]
        public List<Capability> Capabilities { get; set; }
    }
}