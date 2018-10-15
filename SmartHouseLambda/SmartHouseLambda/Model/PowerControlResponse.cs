using Newtonsoft.Json;

namespace SmartHouseLambda.Model
{
    public class PowerControlResponse : BaseResponse
    {
        [JsonProperty("context")]
        public Context Context { get; set; }
    }
}