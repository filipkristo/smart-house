using System;
using System.Threading.Tasks;

using Amazon.Lambda.Core;
using Newtonsoft.Json;
using SmartHouseLambda.Controllers;
using SmartHouseLambda.Model;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace SmartHouseLambda
{
    public class Function
    {
        public async Task<BaseResponse> FunctionHandler(SmartHomeRequest input, ILambdaContext context)
        {
            try
            {
                LambdaLogger.Log(JsonConvert.SerializeObject(input));

                var controller = ControllerFactory.GetController(input);
                return await controller.HandleAlexaRequest(input, context).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                LambdaLogger.Log($"Unhabdled exception: {ex}");
                return null;
            }
        }
    }
}
