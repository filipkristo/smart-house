using System;
using System.Threading.Tasks;
using SmartHouseAbstraction.Core.Command;

namespace SmartHouseCore.Command
{
    public class CommandInvoker : ICommandInvoker
    {
        public Task InvokeAsync(string commandName, string deviceName) => Task.CompletedTask;        
    }
}
