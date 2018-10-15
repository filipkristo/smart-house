using Newtonsoft.Json;

namespace SmartHouseLambda.Model
{
    public class HsvColor
    {
        [JsonProperty("hue")]
        public float Hue { get; set; }

        [JsonProperty("saturation")]
        public float Saturation { get; set; }

        [JsonProperty("brightness")]
        public float Brightness { get; set; }
    }
}