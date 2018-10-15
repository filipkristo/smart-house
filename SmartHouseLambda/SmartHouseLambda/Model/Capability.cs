using System.Collections.Generic;
using Newtonsoft.Json;

namespace SmartHouseLambda.Model
{
    /// <summary>
    /// A capability is a polymorphic type. Currently, AlexaInterface is the only supported type of capability.
    /// </summary>
    /// <remarks>
    /// Describes the actions an endpoint is capable of performing and the properties that can be retrieved and report change notifications.
    /// </remarks>
    /// <see cref="https://developer.amazon.com/docs/device-apis/alexa-discovery.html#capability-object"/>
    public class Capability
    {
        /// <summary>
        /// Indicates the type of capability, which determines what fields the capability has.
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }

        /// <summary>
        /// The qualified name of the interface that describes the actions for the device.
        /// </summary>
        [JsonProperty("interface")]
        public string Interface { get; set; }

        /// <summary>
        /// Indicates the interface version that this endpoint supports.
        /// </summary>
        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("properties")]
        public Properties Properties { get; set; }

        [JsonProperty("inputs")]
        public IEnumerable<Input> Inputs { get; set; }

        [JsonProperty("supportsDeactivation")]
        public bool? SupportsDeactivation { get; set; }

        [JsonProperty("proactivelyReported")]
        public bool? ProactivelyReported { get; set; }

        [JsonProperty("retrievable")]
        public bool? Retrievable { get; set; }

        //[JsonProperty("cameraStreamConfigurations")]
        //public List<CameraStreamConfiguration> cameraStreamConfigurations { get; set; }

        /// <summary>
        /// User for cooking
        /// </summary>
        [JsonProperty("configuration")]
        public CookingConfiguration Configuration;

        [JsonProperty("supportedOperations")]
        public IEnumerable<string> SupportedOperations { get; set; }
    }

    public class CookingConfiguration
    {
        [JsonProperty("supportsRemoteStart")]
        public bool SupportsRemoteStart { get; set; }

        [JsonProperty("enumeratedPowerLevels")]
        public List<string> EnumeratedPowerLevels { get; set; }

        [JsonProperty("integralPowerLevels")]
        public List<int> IntegralPowerLevels { get; set; }

        [JsonProperty("supportedCookingModes")]
        public List<string> SupportedCookingModes { get; set; }
    }
}