using System;
using System.Threading.Tasks;
using SmartHouseDto.Command;

namespace SmartHouseAbstraction.DataStore.Service
{
    public interface ICommandService
    {
        Task<Command> GetCommandAsync(int id);

        Task<Command> GetCommandByNameAsnc(string name);
    }
}
