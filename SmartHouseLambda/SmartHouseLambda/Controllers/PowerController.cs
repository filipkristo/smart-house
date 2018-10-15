using Amazon.Lambda.Core;
using SmartHouseLambda.Const;
using SmartHouseLambda.Controllers.Interfaces;
using SmartHouseLambda.Model;
using SmartHouseLambda.Model.PropertyModels;
using SmartHouseLambda.Service;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SmartHouseLambda.Controllers
{
    public class PowerController : BaseController
    {
        public PowerController(string token) : base(token)
        {

        }

        public override Task<BaseResponse> HandleAlexaRequest(SmartHomeRequest request, ILambdaContext context)
        {
            switch (request.Directive.Header.Name)
            {
                case "TurnOn":
                    return HandleTurnOn(request);
                case "TurnOff":
                    return HandleTurnOff(request);
                default:
                    return Task.FromResult<BaseResponse>(default);
            }
        }

        private async Task<BaseResponse> HandleTurnOn(SmartHomeRequest request)
        {
            if(request.Directive.Endpoint.EndpointId == DeviceConst.SmartHouseDeviceId)
            {
                await _service.TurnOnSmartHouse().ConfigureAwait(false);
            }
            else if(request.Directive.Endpoint.EndpointId == DeviceConst.AirConditionerDeviceId)
            {
                await _service.TurnOnAirConditioner().ConfigureAwait(false);
            }

            return new PowerControlResponse
            {
                Context = new Context
                {
                    Properties = new List<Property>
                    {
                        new StringValueProperty
                        {
                            Namespace = "Alexa.PowerController",
                            Name = "powerState",
                            Value = "ON",
                            TimeOfSample = DateTime.UtcNow,
                            UncertaintyInMilliseconds = 600,
                        }
                    }
                },
                Event = ConstructReponseEvent(request.Directive, "Response"),
            };
        }

        private async Task<BaseResponse> HandleTurnOff(SmartHomeRequest request)
        {
            if (request.Directive.Endpoint.EndpointId == DeviceConst.SmartHouseDeviceId)
            {
                await _service.TurnOffSmartHouse().ConfigureAwait(false);
            }
            else if (request.Directive.Endpoint.EndpointId == DeviceConst.AirConditionerDeviceId)
            {
                await _service.TurnOffAirConditioner().ConfigureAwait(false);
            }

            return new PowerControlResponse
            {
                Context = new Context
                {
                    Properties = new List<Property>
                    {
                        new StringValueProperty
                        {
                            Namespace = "Alexa.PowerController",
                            Name = "powerState",
                            Value = "OFF",
                            TimeOfSample = DateTime.UtcNow,
                            UncertaintyInMilliseconds = 600,
                        }
                    },
                },
                Event = ConstructReponseEvent(request.Directive, "Response"),
            };
        }
    }
}
