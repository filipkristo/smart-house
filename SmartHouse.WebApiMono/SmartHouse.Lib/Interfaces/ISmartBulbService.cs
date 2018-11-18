using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHouse.Lib
{
    public interface ISmartBulbService
    {
        Task Initialize();

        Task PowerOn();

        Task PowerOff();

        Task SetRed();

        Task SetWhite();

        Task Toggle();

        Task<bool> IsTurnOn();
    }
}
