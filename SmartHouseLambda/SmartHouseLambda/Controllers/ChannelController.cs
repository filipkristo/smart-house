using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using SmartHouseLambda.Model;

namespace SmartHouseLambda.Controllers
{
    public class ChannelController : BaseController
    {
        public ChannelController(string token) : base(token)
        {
        }

        public override Task<BaseResponse> HandleAlexaRequest(SmartHomeRequest request, ILambdaContext context)
        {
            throw new NotImplementedException();
        }
    }
}
