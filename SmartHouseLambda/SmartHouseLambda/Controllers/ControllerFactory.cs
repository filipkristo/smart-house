using SmartHouseLambda.Controllers.Interfaces;
using SmartHouseLambda.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartHouseLambda.Controllers
{
    public static class ControllerFactory
    {
        public static IAlexaSmartHomeController GetController(SmartHomeRequest request)
        {
            switch(request.Directive.Header.Namespace)
            {
                case "Alexa.Discovery":
                    return new DiscoveryController();
                case "Alexa.PowerController":
                    return new PowerController(request.Directive.Endpoint.Scope.Token);
                case "Alexa.Speaker":
                    return new SpeakerController(request.Directive.Endpoint.Scope.Token);
                case "Alexa.ChannelController":
                    return new ChannelController(request.Directive.Endpoint.Scope.Token);
                case "Alexa.PlaybackController":
                    return new PlaybackController(request.Directive.Endpoint.Scope.Token);
                case "Alexa.InputController":
                    return new InputController(request.Directive.Endpoint.Scope.Token);
                default:
                    throw new NotImplementedException("Can't find controller");
            }
        }
    }
}
