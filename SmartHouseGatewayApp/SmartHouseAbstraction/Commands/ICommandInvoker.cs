using System.Threading.Tasks;

namespace SmartHouseCoreAbstraction.Commands
{
    public interface ICommandInvoker
    {
        Task InvokeAsync(string commandName, string deviceName);
    }
}
