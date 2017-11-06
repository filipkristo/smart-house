using SmartHouse.UWPLib.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHouse.UWPClient.ViewModels
{
    public class TorrentViewModel : BaseWebViewViewModel
    {
        public TorrentViewModel()
            : base($"http://{SettingsService.Instance.HostIP}/rutorrent/")
        {
        }
    }
}
