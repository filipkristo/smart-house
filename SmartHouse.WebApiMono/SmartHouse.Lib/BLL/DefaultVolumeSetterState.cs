using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHouse.Lib
{
    public static class DefaultVolumeSetterState
    {
        private static DateTime settingDefaultVolumeState;

        public static bool IsDefaultVolumeSetted() => settingDefaultVolumeState == DateTime.Today;
        public static void DefaultVolumeSetted() => settingDefaultVolumeState = DateTime.Today;
            
    }
}
