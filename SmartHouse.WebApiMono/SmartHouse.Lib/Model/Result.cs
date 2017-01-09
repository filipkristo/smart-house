using Newtonsoft.Json;
using System;
namespace SmartHouse.Lib
{
	public class Result
	{
        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

        [JsonProperty(PropertyName = "ok")]
        public bool Ok { get; set; }

        [JsonProperty(PropertyName = "errorCode")]
        public int ErrorCode { get; set; }

		public Result()
		{
		}
	}
}
