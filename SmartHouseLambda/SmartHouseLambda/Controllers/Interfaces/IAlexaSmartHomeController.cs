using Amazon.Lambda.Core;
using SmartHouseLambda.Model;
using SmartHouseLambda.Model.PropertyModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SmartHouseLambda.Controllers.Interfaces
{
    public interface IAlexaSmartHomeController
    {
        Task<BaseResponse> HandleAlexaRequest(SmartHomeRequest request, ILambdaContext context);
    }
}
