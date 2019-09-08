using System.Threading.Tasks;
using SmartHouseCoreAbstraction.Commands;
using SmartHouseDataStoreAbstraction.Commands;

namespace SmartHouseCore.Commands
{
    public class CommandInvoker : ICommandInvoker
    {
        private readonly ICommandService _commandService;

        public CommandInvoker(ICommandService commandService)
        {
            _commandService = commandService;
        }

        public async Task InvokeAsync(string commandName, string deviceName)
        {
            var command = await _commandService.GetCommandByNameAsnc(commandName, deviceName);
        }
    }
}
