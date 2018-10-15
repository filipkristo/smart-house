using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using SmartHouseLambda.Model;
using SmartHouseLambda.Model.PropertyModels;

namespace SmartHouseLambda.Controllers
{
    public class SpeakerController : BaseController
    {
        public SpeakerController(string token) : base(token)
        {

        }

        public override async Task<BaseResponse> HandleAlexaRequest(SmartHomeRequest request, ILambdaContext context)
        {
            if (request.Directive.Payload.Mute)
                await _service.Mute().ConfigureAwait(false);
            else if(request.Directive.Payload.Volume > 0)
                await _service.VolumeUp().ConfigureAwait(false);
            else if (request.Directive.Payload.Volume < 0)
                await _service.VolumeDown().ConfigureAwait(false);

            return new PowerControlResponse
            {
                Context = new Context
                {
                    Properties = new List<Property>
                    {
                        new StringValueProperty
                        {
                            Namespace = "Alexa.Speaker",
                            Name = "volume",
                            Value = request.Directive.Payload.Volume.ToString(),
                            TimeOfSample = DateTime.UtcNow,
                            UncertaintyInMilliseconds = 0,
                        },
                        new StringValueProperty
                        {
                            Namespace = "Alexa.Speaker",
                            Name = "muted",
                            Value = request.Directive.Payload.Mute.ToString(),
                            TimeOfSample = DateTime.UtcNow,
                            UncertaintyInMilliseconds = 0,
                        }
                    }
                },
                Event = ConstructReponseEvent(request.Directive, "Response"),
            };
        }
    }
}
