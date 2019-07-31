using System;
using SmartHouseAbstraction.DataStore.Model;

namespace SmartHouseDataStore.Entities
{
    public class Command : IEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
