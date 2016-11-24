using SmartHouse.UWPClient.Services.SettingsServices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Template10.Services.KeyboardService;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace SmartHouse.UWPClient.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RemoteController : Page
    {
        private bool loaded = false;

        public RemoteController()
        {
            this.InitializeComponent();
            NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
            
            webView.PermissionRequested += webView_PermissionRequested;
            webView.ContainsFullScreenElementChanged += webView_ContainsFullScreenElementChanged;
            webView.LoadCompleted += WebView_LoadCompleted;

            webView.Navigate(new Uri($"http://{SettingsService.Instance.HostIP}/smarthouse/"));

            var KeyboardHelper = new KeyboardHelper();
            KeyboardHelper.KeyDown = new Action<KeyboardEventArgs>(RemoteController_KeyDown);
        }

        private async void RemoteController_KeyDown(KeyboardEventArgs e)
        {
            Debug.WriteLine((int)e.VirtualKey);

            var result = "";
            var keyCode = (int)e.VirtualKey;
            
            switch (keyCode)
            {
                case 179:
                    result = await webView.InvokeScriptAsync("runCommand", new[] { "Play" });
                    break;
                case 176:
                    result = await webView.InvokeScriptAsync("runCommand", new[] { "Next" });                   
                    break;
                case 177:
                    result = await webView.InvokeScriptAsync("runCommand", new[] { "Prev" });
                    break;
                case 175:
                    result = await webView.InvokeScriptAsync("runCommand", new[] { "VolumeUp" });                    
                    break;
                case 174:
                    result = await webView.InvokeScriptAsync("runCommand", new[] { "VolumeDown" });                    
                    break;
                case 49:
                    result = await webView.InvokeScriptAsync("runCommand", new[] { "Pandora" });                    
                    break;
                case 50:
                    result = await webView.InvokeScriptAsync("runCommand", new[] { "Music" });
                    break;
                case 51:
                    result = await webView.InvokeScriptAsync("runCommand", new[] { "XBox" });
                    break;
                case 52:
                    result = await webView.InvokeScriptAsync("runCommand", new[] { "TV" });
                    break;
                case 76:
                    result = await webView.InvokeScriptAsync("love", null);
                    break;
                case 73:
                    result = await webView.InvokeScriptAsync("toogleDialog", null);
                    break;
            }
        }

        private void WebView_LoadCompleted(object sender, NavigationEventArgs e)
        {
            loaded = true;
        }

        private void webView_PermissionRequested(WebView sender, WebViewPermissionRequestedEventArgs args)
        {
            if (args.PermissionRequest.PermissionType == WebViewPermissionType.Geolocation)
            {
                args.PermissionRequest.Allow();
            }
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            if(loaded)
                await webView.InvokeScriptAsync("refreshAll", null);            
        }

        private void webView_ContainsFullScreenElementChanged(WebView sender, object args)
        {
            var applicationView = ApplicationView.GetForCurrentView();

            if (sender.ContainsFullScreenElement)
            {
                applicationView.TryEnterFullScreenMode();
            }
            else if (applicationView.IsFullScreenMode)
            {
                applicationView.ExitFullScreenMode();
            }
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            webView.Navigate(new Uri($"http://{SettingsService.Instance.HostIP}/smarthouse/"));
        }
    }
}
