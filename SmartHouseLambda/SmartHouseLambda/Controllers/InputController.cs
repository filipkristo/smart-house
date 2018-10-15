using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using SmartHouseLambda.Model;
using SmartHouseLambda.Model.PropertyModels;

namespace SmartHouseLambda.Controllers
{
    public class InputController : BaseController
    {
        public InputController(string token) : base(token)
        {
        }

        public override async Task<BaseResponse> HandleAlexaRequest(SmartHomeRequest request, ILambdaContext context)
        {
            if(request.Directive.Payload.Input.Equals("Radio"))
            {
                await _service.Pandora().ConfigureAwait(false);
            }
            else if(request.Directive.Payload.Input.Equals("TV"))
            {
                await _service.TV().ConfigureAwait(false);
            }

            return new PowerControlResponse
            {
                Context = new Context
                {
                    Properties = new List<Property>
                    {
                        new StringValueProperty
                        {
                            Namespace = "Alexa.InputController",
                            Name = "input",
                            Value = request.Directive.Payload.Input,
                            TimeOfSample = DateTime.UtcNow,
                            UncertaintyInMilliseconds = 600,
                        }
                    }
                },
                Event = ConstructReponseEvent(request.Directive, "Response"),
            };
        }
    }
}
