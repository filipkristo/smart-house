using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartHouseLambda.Model
{
    public class BaseCommandResponse : BaseResponse
    {
        [JsonProperty("context")]
        public Context Context { get; set; }
    }
}
