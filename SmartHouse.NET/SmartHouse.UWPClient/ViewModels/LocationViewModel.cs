using SmartHouse.UWPClient.Services.SettingsServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHouse.UWPClient.ViewModels
{
    public class LocationViewModel : BaseWebViewViewModel
    {
        public LocationViewModel()
            :base($"http://{SettingsService.Instance.HostIP}/location.php")
        {

        }
    }
}
