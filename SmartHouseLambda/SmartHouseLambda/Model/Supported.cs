using Newtonsoft.Json;

namespace SmartHouseLambda.Model
{
    public class Supported
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}