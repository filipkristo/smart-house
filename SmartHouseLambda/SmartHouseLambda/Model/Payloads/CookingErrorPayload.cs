using Newtonsoft.Json;

namespace SmartHouseLambda.Model.Payloads
{
    public class CookingErrorPayload : ErrorPayload
    {
        [JsonProperty("maxCookTime")]
        public string MaxCookTime { get; set; }
    }
}