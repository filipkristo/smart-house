using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace SmartHouseDataStoreTests
{
    [CollectionDefinition("Database collection")]
    public class DatabaseCollection : ICollectionFixture<InMemoryContextFixture>
    {
    }
}
