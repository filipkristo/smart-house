using SmartHouse.UWPLib.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHouse.UWPClient.ViewModels
{
    public class MPDClientViewModel : BaseWebViewViewModel
    {
        public MPDClientViewModel()
            :base($"http://{SettingsService.Instance.HostIP}/MPD3/")
        {

        }

    }
}
