using OrviboController.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHouse.Lib
{
    public interface IOrviboService
    {
        Result TurnOn();
        Result TurnOff();
        Device GetDevice();
    }
}
