using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using SmartHouseDataStore;
using System.Linq;

namespace SmartHouseDataStoreTests.SeedContextTests
{
    [Collection("Database collection")]
    public class DeviceTypeTests
    {
        private readonly SmartHouseContext _smartHouseContext;

        public DeviceTypeTests(InMemoryContextFixture inMemoryContextFixture)
        {
            _smartHouseContext = inMemoryContextFixture.Context;
        }
        [Fact]
        public void Should_DeviceType_Amplifier_Exists()
        {
            var exists = _smartHouseContext.
                DeviceTypes.
                Any(x => x.Name == "Amplifier");

            Assert.True(exists);
        }

        [Fact]
        public void Should_DeviceType_TV_Exists()
        {
            var exists = _smartHouseContext.
                DeviceTypes.
                Any(x => x.Name == "TV");

            Assert.True(exists);
        }

        [Fact]
        public void Should_DeviceType_Music_Player_Exists()
        {
            var exists = _smartHouseContext.
                DeviceTypes.
                Any(x => x.Name == "Music Player");

            Assert.True(exists);
        }

        [Fact]
        public void Should_DeviceType_Lights_Exists()
        {
            var exists = _smartHouseContext.
                DeviceTypes.
                Any(x => x.Name == "Lights");

            Assert.True(exists);
        }

        [Fact]
        public void Should_DeviceType_Switch_Exists()
        {
            var exists = _smartHouseContext.
                DeviceTypes.
                Any(x => x.Name == "Switch");

            Assert.True(exists);
        }

        [Fact]
        public void Should_DeviceType_Telemetry_Device_Exists()
        {
            var exists = _smartHouseContext.
                DeviceTypes.
                Any(x => x.Name == "Telemetry Device");

            Assert.True(exists);
        }
    }
}
