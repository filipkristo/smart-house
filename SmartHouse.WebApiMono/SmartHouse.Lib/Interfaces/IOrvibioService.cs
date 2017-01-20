using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHouse.Lib
{
    public interface IOrvibioService
    {
        Result TurnOn();
        Result TurnOff();     
    }
}
