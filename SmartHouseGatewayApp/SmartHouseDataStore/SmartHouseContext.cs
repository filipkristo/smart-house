using Microsoft.EntityFrameworkCore;
using SmartHouseDataStore.Entities;

namespace SmartHouseDataStore
{
    public class SmartHouseContext : DbContext
    {
        public DbSet<Command> Commands { get; set; }
        public DbSet<Condition> Conditions { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<DeviceSetting> DeviceSettings { get; set; }
        public DbSet<DeviceState> DeviceStates { get; set; }
        public DbSet<DeviceType> DeviceTypes { get; set; }
        public DbSet<Routine> Routines { get; set; }
        public DbSet<RoutineCommand> RoutineCommands { get; set; }
        public DbSet<SmartHouseState> SmartHouseStates { get;set; }
    }
}
