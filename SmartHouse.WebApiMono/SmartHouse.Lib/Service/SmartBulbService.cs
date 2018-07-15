using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YeelightAPI;

namespace SmartHouse.Lib
{
    public class SmartBulbService : ISmartBulbService, IDisposable
    {
        private const int _delay = 1500;
        private DeviceGroup _deviceGroup = null;

        public async Task Initialize()
        {
            if(_deviceGroup == null)
            {
                var devices = await DeviceLocator.Discover().ConfigureAwait(false);
                if (devices.Count > 0)
                {
                    _deviceGroup = new DeviceGroup(devices);
                    await _deviceGroup.Connect().ConfigureAwait(false);
                }
            }
        }

        public Task PowerOn() => _deviceGroup?.TurnOn(_delay);

        public Task PowerOff() => _deviceGroup?.TurnOff(_delay);

        public Task Toggle() => _deviceGroup?.Toggle();

        public async Task SetWhite()
        {
            if(_deviceGroup != null)
            {
                await _deviceGroup.SetDefault().ConfigureAwait(false);
                await _deviceGroup.SetRGBColor(255, 255, 255, _delay).ConfigureAwait(false);
                await _deviceGroup.SetBrightness(80, _delay).ConfigureAwait(false);
            }
        }

        public async Task SetRed()
        {
            if(_deviceGroup != null)
            {
                await _deviceGroup.SetRGBColor(185, 48, 242, _delay).ConfigureAwait(false);
                await _deviceGroup.SetBrightness(50, _delay).ConfigureAwait(false);
            }
        }

        public void Dispose()
        {
            _deviceGroup?.Dispose();
            _deviceGroup = null;
        }        
    }
}
