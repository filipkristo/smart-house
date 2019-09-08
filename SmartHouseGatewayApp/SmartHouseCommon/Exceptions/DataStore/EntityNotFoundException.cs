using System;
namespace SmartHouseCommon.Exceptions.DataStore
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(string message) : base(message)
        {
        }
    }
}
