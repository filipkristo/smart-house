using System;
using Newtonsoft.Json;

namespace SmartHouseGatewayApp.Middlewares
{
    public class ErrorDetails
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
