using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using SmartHouseLambda.Model;
using SmartHouseLambda.Model.PropertyModels;

namespace SmartHouseLambda.Controllers
{
    public class PlaybackController : BaseController
    {
        public PlaybackController(string token) : base(token)
        {
        }

        public override async Task<BaseResponse> HandleAlexaRequest(SmartHomeRequest request, ILambdaContext context)
        {
            if(request.Directive.Header.Name.Equals("Play"))
            {
                await _service.Play().ConfigureAwait(false);
            }
            else if(request.Directive.Header.Name.Equals("Pause"))
            {
                await _service.Pause().ConfigureAwait(false);
            }
            else if(request.Directive.Header.Name.Equals("Next"))
            {
                await _service.Next().ConfigureAwait(false);
            }
            else if (request.Directive.Header.Name.Equals("Stop"))
            {
                await _service.Pause().ConfigureAwait(false);
            }

            return new BaseCommandResponse()
            {
                Context = new Context
                {
                    Properties = new List<Property>()
                },
                Event = ConstructReponseEvent(request.Directive, "Response"),
            };
        }
    }
}
