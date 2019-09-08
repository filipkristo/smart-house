using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SmartHouseCommon.Exceptions.DataStore;
using SmartHouseDataStoreAbstraction.Commands;
using SmartHouseDto.Commands;

namespace SmartHouseDataStore.Service
{
    public class CommandService : ICommandService
    {
        private readonly SmartHouseContext _smartHouseContext;
        private readonly IMapper _mapper;

        public CommandService(SmartHouseContext smartHouseContext, IMapper mapper)
        {
            _smartHouseContext = smartHouseContext;
            _mapper = mapper;
        }

        public async Task<Command> GetCommandAsync(int id)
        {
            var entity = await _smartHouseContext
                .Commands
                .Include(x => x.Device)
                .Include(x => x.Device.DeviceSettings)               
                .Include(x => x.Device.DeviceType)
                .Include(x => x.Device.DeviceStates)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
                throw new EntityNotFoundException($"Command entity with id '{id}' doesn't exists");

            return _mapper.Map<Command>(entity);
        }

        public async Task<Command> GetCommandByNameAsnc(string name, string deviceName)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            if (deviceName == null)
                throw new ArgumentNullException(nameof(deviceName));

            var entity = await _smartHouseContext
                .Commands
                .Include(x => x.Device)
                .Include(x => x.Device.DeviceSettings)
                .Include(x => x.Device.DeviceType)
                .Include(x => x.Device.DeviceStates)
                .FirstOrDefaultAsync(x => x.Name == name && x.Device.Name == deviceName);

            if (entity == null)
                throw new EntityNotFoundException($"Command entity with name '{name}' and device {deviceName} doesn't exists");

            return _mapper.Map<Command>(entity);
        }
    }
}
