using System;
using Newtonsoft.Json;

namespace SmartHouseLambda.Model.PropertyModels
{
    /// <summary>
    /// TODO: Subclass for string value or numeric value.
    /// </summary>
    public abstract class Property
    {
        [JsonProperty("namespace")]
        public string Namespace { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("timeOfSample")]
        public DateTime TimeOfSample { get; set; }

        [JsonProperty("uncertaintyInMilliseconds")]
        public int UncertaintyInMilliseconds { get; set; }
    }
}