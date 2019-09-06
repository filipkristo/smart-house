using System;
using System.Linq;
using Xunit;

namespace SmartHouseDataStoreTests
{
    public class SeedContextTests : IClassFixture<InMemoryContextFixture>
    {
        private readonly InMemoryContextFixture _inMemoryContextFixture;

        public SeedContextTests(InMemoryContextFixture inMemoryContextFixture)
        {
            _inMemoryContextFixture = inMemoryContextFixture;
        }

        [Fact]
        public void Should_DeviceType_Amplifier_Exists()
        {
            var exists = _inMemoryContextFixture.
                Context.
                DeviceTypes.
                Any(x => x.Name == "Amplifier");

            Assert.True(exists);
        }

        [Fact]
        public void Should_DeviceType_TV_Exists()
        {
            var exists = _inMemoryContextFixture.
                Context.
                DeviceTypes.
                Any(x => x.Name == "TV");

            Assert.True(exists);
        }
    }
}
