using System.Collections.Generic;
using Newtonsoft.Json;

namespace SmartHouseLambda.Model
{
    public class Properties
    {
        /// <summary>
        /// Indicates the properties of the interface which are supported by this endpoint in the format "name":"<propertyName>". If you do not specify a reportable property of the interface in this array, the default is to assume that proactivelyReported and retrievable for that property are false.
        /// </summary>
        [JsonProperty("supported")]
        public List<Supported> Supported { get; set; }

        /// <summary>
        /// Indicates whether the properties listed for this endpoint generate Change.Report events.
        /// </summary>
        [JsonProperty("proactivelyReported")]
        public bool ProactivelyReported { get; set; }

        /// <summary>
        /// Indicates whether the properties listed for this endpoint can be retrieved for state reportin
        /// </summary>
        [JsonProperty("retrievable")]
        public bool Retrievable { get; set; }
    }
}