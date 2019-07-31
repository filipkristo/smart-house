using System;
using System.Threading.Tasks;
using SmartHouseAbstraction.DataStore.Service;
using SmartHouseDto.Command;

namespace SmartHouseDataStore.Service
{
    public class CommandService : ICommandService
    {
        public Task<Command> GetCommandAsync(int id) => Task.FromResult(new Command { Id = id });

        public Task<Command> GetCommandByNameAsnc(string name) => Task.FromResult(new Command { Name = name });
    }
}
