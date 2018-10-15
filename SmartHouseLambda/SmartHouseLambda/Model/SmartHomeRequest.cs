using Newtonsoft.Json;

namespace SmartHouseLambda.Model
{
    public class SmartHomeRequest
    {
        [JsonProperty("directive")]
        public Directive Directive { get; set; }
    }
}