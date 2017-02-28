using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHouse.Lib.NamedPipes
{
    public class CommandPipe
    {
        private const string pipeFile = "comfifo";

        public Task Start()
        {
            return Task.Run(async () =>
            {
                while (true)
                {
                    var command = $"cat {pipeFile}";

                    var result = BashHelper.ExecBashCommand(command);

                    if (string.IsNullOrWhiteSpace(result.Error))
                    {
                        var tvService = new TVService();

                        var tvCommand = (IRCommands)Enum.Parse(typeof(IRCommands), result.Message, true);
                        await tvService.SendCommand(tvCommand);
                    }
                }

            });
        }
    }
}
