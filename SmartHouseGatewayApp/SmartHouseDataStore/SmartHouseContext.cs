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

        public SmartHouseContext(DbContextOptions<SmartHouseContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var amplifier = new DeviceType { Id = 1, Name = "Amplifier", Description = string.Empty };
            var tv = new DeviceType { Id = 2, Name = "TV", Description = string.Empty };
            var musicPlayer = new DeviceType { Id = 3, Name = "Music Player", Description = string.Empty };
            var lights = new DeviceType { Id = 4, Name = "Lights", Description = string.Empty };
            var switches = new DeviceType { Id = 5, Name = "Switch", Description = string.Empty };
            var telemetryDevice = new DeviceType { Id = 6, Name = "Telemetry Device", Description = string.Empty };

            modelBuilder.Entity<DeviceType>().HasData(amplifier);
            modelBuilder.Entity<DeviceType>().HasData(tv);
            modelBuilder.Entity<DeviceType>().HasData(musicPlayer);
            modelBuilder.Entity<DeviceType>().HasData(lights);
            modelBuilder.Entity<DeviceType>().HasData(switches);
            modelBuilder.Entity<DeviceType>().HasData(telemetryDevice);

            var deezer = new Device { Id = 1, DeviceTypeId = musicPlayer.Id, Name = "Deezer Player", CrlAssembly = string.Empty, CrlType = string.Empty, Description = string.Empty };
            var pandora = new Device { Id = 2, DeviceTypeId = musicPlayer.Id, Name = "Pandora Player", CrlAssembly = string.Empty, CrlType = string.Empty, Description = string.Empty };
            var yamaha = new Device { Id = 3, DeviceTypeId = amplifier.Id, Name = "Yamaha RX-V481", CrlAssembly = string.Empty, CrlType = string.Empty, Description = string.Empty };
            var philips = new Device { Id = 4, DeviceTypeId = tv.Id, Name = "Philips", CrlAssembly = string.Empty, CrlType = string.Empty, Description = string.Empty };
            var yeelightLivingRoom = new Device { Id = 5, DeviceTypeId = lights.Id, Name = "Yeelight - living room", CrlAssembly = string.Empty, CrlType = string.Empty, Description = string.Empty };
            var yeelightBedroom = new Device { Id = 6, DeviceTypeId = lights.Id, Name = "Yeelight - bedroom", CrlAssembly = string.Empty, CrlType = string.Empty, Description = string.Empty };
            var restartRouter = new Device { Id = 7, DeviceTypeId = switches.Id, Name = "Restart Router", CrlAssembly = string.Empty, CrlType = string.Empty, Description = string.Empty };
            var telemetry = new Device { Id = 8, DeviceTypeId = telemetryDevice.Id, Name = "Telemetry", CrlAssembly = string.Empty, CrlType = string.Empty, Description = string.Empty };

            modelBuilder.Entity<Device>().HasData(deezer);
            modelBuilder.Entity<Device>().HasData(pandora);
            modelBuilder.Entity<Device>().HasData(yamaha);
            modelBuilder.Entity<Device>().HasData(philips);
            modelBuilder.Entity<Device>().HasData(yeelightLivingRoom);
            modelBuilder.Entity<Device>().HasData(yeelightBedroom);
            modelBuilder.Entity<Device>().HasData(restartRouter);
            modelBuilder.Entity<Device>().HasData(telemetry);
        }
    }
}
