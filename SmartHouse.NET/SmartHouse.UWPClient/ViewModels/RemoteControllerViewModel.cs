using SmartHouse.UWPClient.Services.SettingsServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;

namespace SmartHouse.UWPClient.ViewModels
{
    public class RemoteControllerViewModel : BaseWebViewViewModel
    {
        public RemoteControllerViewModel()
            :base($"http://{SettingsService.Instance.HostIP}/smarthouse/")
        {

        }

        public override async void Refresh()
        {
            base.Refresh();
            await WebView?.InvokeScriptAsync("refreshAll", null);
        }

        public async void RefreshTV()
        {
            ItemUrl = null;
            ItemUrl = $"http://{SettingsService.Instance.HostIP}/smarthouseTV/";
            await WebView?.InvokeScriptAsync("refreshAll", null);
        }
    }
}
