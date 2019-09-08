using System.Threading.Tasks;
using SmartHouseDto.Commands;

namespace SmartHouseAbstraction.DataStore.Service
{
    public interface ICommandService
    {
        Task<Command> GetCommandAsync(int id);

        Task<Command> GetCommandByNameAsnc(string name);
    }
}
