using Newtonsoft.Json;

namespace SmartHouseLambda.Model.PropertyModels
{
    public class ColorValueProperty : Property
    {
        [JsonProperty("value")]
        public HsvColor Value { get; set; }
    }
}