using System;
using System.Collections.Generic;
using System.Text;
using SmartHouseDataStore;
using Xunit;
using System.Linq;

namespace SmartHouseDataStoreTests.SeedContextTests
{
    [Collection("Database collection")]
    public class DeviceTests
    {
        private readonly SmartHouseContext _smartHouseContext;

        public DeviceTests(InMemoryContextFixture inMemoryContextFixture)
        {
            _smartHouseContext = inMemoryContextFixture.Context;
        }

        [Fact]
        public void Should_Device_Deezer_Player_Exists()
        {
            var exists = _smartHouseContext.
                Devices.
                Any(x => x.Name == "Deezer Player");

            Assert.True(exists);
        }

        [Fact]
        public void Should_Device_Pandora_Player_Exists()
        {
            var exists = _smartHouseContext.
                Devices.
                Any(x => x.Name == "Pandora Player");

            Assert.True(exists);
        }

        [Fact]
        public void Should_Device_Yamaha_RXV481_Exists()
        {
            var exists = _smartHouseContext.
                Devices.
                Any(x => x.Name == "Yamaha RX-V481");

            Assert.True(exists);
        }

        [Fact]
        public void Should_Device_Philips_Exists()
        {
            var exists = _smartHouseContext.
                Devices.
                Any(x => x.Name == "Philips");

            Assert.True(exists);
        }

        [Fact]
        public void Should_Device_Yeelight_living_room_Exists()
        {
            var exists = _smartHouseContext.
                Devices.
                Any(x => x.Name == "Yeelight - living room");

            Assert.True(exists);
        }

        [Fact]
        public void Should_Device_Yeelight_bedroom_Exists()
        {
            var exists = _smartHouseContext.
                Devices.
                Any(x => x.Name == "Yeelight - bedroom");

            Assert.True(exists);
        }

        [Fact]
        public void Should_Device_Restart_Router_Exists()
        {
            var exists = _smartHouseContext.
                Devices.
                Any(x => x.Name == "Restart Router");

            Assert.True(exists);
        }

        [Fact]
        public void Should_Device_Telemetry_Exists()
        {
            var exists = _smartHouseContext.
                Devices.
                Any(x => x.Name == "Telemetry");

            Assert.True(exists);
        }
    }
}
