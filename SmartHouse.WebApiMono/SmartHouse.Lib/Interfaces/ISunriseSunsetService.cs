using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHouse.Lib
{
    public interface ISunriseSunsetService
    {
        Task<bool> IsNight();
    }
}
