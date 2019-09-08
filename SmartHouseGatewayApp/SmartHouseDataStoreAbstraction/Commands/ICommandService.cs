using System.Threading.Tasks;
using SmartHouseDto.Commands;

namespace SmartHouseDataStoreAbstraction.Commands
{
    public interface ICommandService
    {
        Task<Command> GetCommandAsync(int id);

        Task<Command> GetCommandByNameAsnc(string name, string deviceName);
    }
}
