using Newtonsoft.Json;

namespace SmartHouseLambda.Model.PropertyModels
{
    public class IntValueProperty : Property
    {
        /// <summary>
        /// Might be PropertyValue (25.0, CELSIUS), or string ("HEAT")
        /// </summary>
        [JsonProperty("value")]
        public int Value { get; set; }
    }
}