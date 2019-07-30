using System;
using System.Threading.Tasks;

namespace SmartHouseAbstraction.Core.Command
{
    public interface ICommandInvoker
    {
        Task InvokeAsync(string commandName, string deviceName);
    }
}
