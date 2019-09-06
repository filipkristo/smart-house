using System;
using Microsoft.EntityFrameworkCore;
using SmartHouseDataStore;

namespace SmartHouseDataStoreTests
{
    public class InMemoryContextFixture : IDisposable
    {
        public SmartHouseContext Context { get; }

        public InMemoryContextFixture()
        {
            var options = new DbContextOptionsBuilder<SmartHouseContext>()
                .UseInMemoryDatabase(databaseName: "Add_writes_to_database")
                .Options;

            Context = new SmartHouseContext(options);
            Context.Database.EnsureCreated();
        }

        public void Dispose()
        {
            Context.Dispose();
        }
    }
}
