using System;
using System.Threading.Tasks;
using SmartHouseDeviceAbstraction.Lights;

namespace SmartHouseDevice.Lights
{
    public class LightBulbService : ILightBulbService
    {
        public Task ToogleAsync() => Task.CompletedTask;

        public Task TurnOffAsync() => Task.CompletedTask;

        public Task TurnOnAsync() => Task.CompletedTask;
    }
}
