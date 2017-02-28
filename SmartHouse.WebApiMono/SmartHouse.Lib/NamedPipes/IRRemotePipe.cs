using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHouse.Lib
{
    public class IRRemotePipe
    {
        private const string pipeFile = "tvcom";

        public Task Start()
        {
            return Task.Run(() =>
            {
                while(true)
                {
                    var command = $"cat {pipeFile}";

                    var result = BashHelper.ExecBashCommand(command);

                    if(string.IsNullOrWhiteSpace(result.Error))
                    {
                        var tvService = new TVService();
                        var tvCommand = (NPCommands.Commands)Enum.Parse(typeof(NPCommands.Commands), result.Message);

                        switch (tvCommand)
                        {
                            case NPCommands.Commands.VolumeUp:
                                break;
                            case NPCommands.Commands.VolumeDown:
                                break;
                            case NPCommands.Commands.Forward:
                                break;
                            case NPCommands.Commands.Rewind:
                                break;
                            case NPCommands.Commands.Power:
                                break;
                            case NPCommands.Commands.Play:
                                break;
                            case NPCommands.Commands.Pause:
                                break;
                            case NPCommands.Commands.Stop:
                                break;
                            case NPCommands.Commands.Up:
                                break;
                            case NPCommands.Commands.Down:
                                break;
                            case NPCommands.Commands.Left:
                                break;
                            case NPCommands.Commands.Right:
                                break;
                            case NPCommands.Commands.Ok:
                                break;
                            case NPCommands.Commands.Love:
                                break;
                            case NPCommands.Commands.Ban:
                                break;
                            case NPCommands.Commands.Pandora:
                                break;
                            case NPCommands.Commands.Music:
                                break;
                            case NPCommands.Commands.XBox:
                                break;
                            case NPCommands.Commands.TV:
                                break;
                            default:
                                break;
                        }
                    }
                }

            });
        }
    }
}
