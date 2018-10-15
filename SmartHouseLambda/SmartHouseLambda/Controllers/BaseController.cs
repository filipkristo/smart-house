using Amazon.Lambda.Core;
using SmartHouseLambda.Controllers.Interfaces;
using SmartHouseLambda.Model;
using SmartHouseLambda.Service;
using SmartHouseLambda.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SmartHouseLambda.Controllers
{
    public abstract class BaseController : IAlexaSmartHomeController
    {
        protected readonly ISmartHouseService _service;

        protected BaseController(string token)
        {
            _service = new SmartHouseService(token);
        }

        public abstract Task<BaseResponse> HandleAlexaRequest(SmartHomeRequest request, ILambdaContext context);

        protected Event ConstructReponseEvent(Directive directive, string name)
        {
            return new Event
            {
                Header = new Header
                {
                    MessageId = directive.Header.MessageId,
                    CorrelationToken = directive.Header.CorrelationToken,
                    Namespace = "Alexa",
                    Name = name,
                    PayloadVersion = "3",
                },
                Endpoint = new StateReportEndpoint
                {
                    EndpointId = directive.Endpoint.EndpointId,
                    Scope = new Scope
                    {
                        Type = directive.Endpoint.Scope.Type,
                        Token = directive.Endpoint.Scope.Token,
                    }
                },
            };
        }
    }
}
