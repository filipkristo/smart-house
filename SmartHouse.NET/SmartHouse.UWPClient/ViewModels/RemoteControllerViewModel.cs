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
        private bool refreshed;

        public RemoteControllerViewModel()
            :base($"http://{SettingsService.Instance.HostIP}/smarthouse/")
        {

        }

        public override async void Refresh()
        {
            refreshed = false;
            base.Refresh();

            if(refreshed)
                await WebView?.InvokeScriptAsync("refreshAll", null);

            refreshed = true;
        }

        public async void RefreshTV()
        {
            refreshed = false;
            ItemUrl = null;

            ItemUrl = $"http://{SettingsService.Instance.HostIP}/smarthouseTV/";

            if(refreshed)
                await WebView?.InvokeScriptAsync("refreshAll", null);

            refreshed = true;
        }
    }
}
