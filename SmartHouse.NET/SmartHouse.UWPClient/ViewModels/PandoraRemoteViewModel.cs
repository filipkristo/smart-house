using SmartHouse.UWPClient.Services.SettingsServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHouse.UWPClient.ViewModels
{
    public class PandoraRemoteViewModel : BaseWebViewViewModel
    {
        public PandoraRemoteViewModel()
            : base($"http://{SettingsService.Instance.HostIP}/player/pandora.php")
        {

        }        
    }
}
