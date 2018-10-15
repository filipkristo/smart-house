using Amazon.Lambda.Core;
using SmartHouseLambda.Const;
using SmartHouseLambda.Controllers.Interfaces;
using SmartHouseLambda.Model;
using SmartHouseLambda.Model.PropertyModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SmartHouseLambda.Controllers
{
    public class DiscoveryController : IAlexaSmartHomeController
    {
        public Task<BaseResponse> HandleAlexaRequest(SmartHomeRequest request, ILambdaContext context)
        {
            DiscoverResponse response = new DiscoverResponse
            {
                Event = new Event
                {
                    Header = new Header
                    {
                        MessageId = request.Directive.Header.MessageId,
                        Namespace = "Alexa.Discovery",
                        Name = "Discover.Response",
                        PayloadVersion = "3"
                    },
                    Payload = new EventPayload
                    {
                        Endpoints = GetEndpoints()
                    }
                }
            };

            return Task.FromResult<BaseResponse>(response);
        }

        private static IEnumerable<Endpoint> GetEndpoints()
        {
            return new List<Endpoint>()
            {
                new Endpoint
                {
                    EndpointId = DeviceConst.SmartHouseDeviceId,
                    Description = "Remote controller for smart house",
                    FriendlyName = "Smart house",
                    ManufacturerName = "Filip Kristo",
                    DisplayCategories = new[]{ "SPEAKER" },
                    Capabilities = new List<Capability>
                    {
                        new Capability
                        {
                            Type = "AlexaInterface",
                            Interface = "Alexa.Speaker",
                            Version = "3",
                            Properties = new Properties
                            {
                                Supported = new List<Supported>
                                {
                                    new Supported
                                    {
                                        Name = "volume"
                                    },
                                    new Supported
                                    {
                                        Name = "muted"
                                    }
                                }
                            }
                        },
                        new Capability
                        {
                            Type = "AlexaInterface",
                            Interface = "Alexa.ChannelController",
                            Version = "3",
                            Properties = new Properties
                            {
                                Supported = new List<Supported>
                                {
                                    new Supported
                                    {
                                        Name = "channel"
                                    }
                                }
                            }
                        },
                        new Capability
                        {
                            Type = "AlexaInterface",
                            Interface = "Alexa.PlaybackController",
                            Version = "3",
                            Properties = new Properties(),
                            SupportedOperations = new []
                            {
                                "Play",
                                "Pause",
                                "Stop",
                                "Next"
                            }
                        },
                        new Capability
                        {
                            Type = "AlexaInterface",
                            Interface = "Alexa.InputController",
                            Version = "3",
                            Properties = new Properties
                            {
                                Supported = new List<Supported>
                                {
                                    new Supported
                                    {
                                        Name = "input"
                                    }
                                }
                            },
                            Inputs = new[]
                            {
                                new Input
                                {
                                    Name = "Radio"
                                },
                                new Input
                                {
                                    Name = "TV"
                                }
                            }
                        },
                        new Capability
                        {
                            Type = "AlexaInterface",
                            Interface = "Alexa.PowerController",
                            Version = "3",
                            Properties = new Properties
                            {
                                Supported = new List<Supported>
                                {
                                    new Supported
                                    {
                                        Name = "powerState"
                                    }
                                }
                            },
                            Retrievable = true,
                            ProactivelyReported = true
                        },
                    }
                },
                new Endpoint
                {
                    EndpointId = DeviceConst.AirConditionerDeviceId,
                    Description = "Remote controller for air conditioner",
                    FriendlyName = "Air conditioner",
                    ManufacturerName = "Filip Kristo",
                    DisplayCategories = new []{ "SWITCH" },
                    Capabilities = new List<Capability>
                    {
                        new Capability
                        {
                            Type = "AlexaInterface",
                            Interface = "Alexa.PowerController",
                            Version = "3",
                            Properties = new Properties
                            {
                                Supported = new List<Supported>
                                {
                                    new Supported
                                    {
                                        Name = "powerState"
                                    }
                                }
                            },
                            Retrievable = true,
                            ProactivelyReported = true
                        },
                    }
                }
            };
        }

    }
}
