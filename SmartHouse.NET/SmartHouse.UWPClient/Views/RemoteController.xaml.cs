using SmartHouse.UWPLib.Service;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Template10.Common;
using Template10.Services.KeyboardService;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Core;
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
        public RemoteController()
        {
            this.InitializeComponent();
            NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;

            BootStrapper.BackRequested += BootStrapper_BackRequested;
            webView.ScriptNotify += WebView_ScriptNotify;
        }

        async private void WebView_ScriptNotify(object sender, NotifyEventArgs e)
        {
            try
            {
                string data = e.Value;
                if (data.ToLower().StartsWith("launchlink:https://www.last.fm"))
                {
                    await Launcher.LaunchUriAsync(new Uri(data.Substring("launchlink:".Length), UriKind.Absolute));
                }
            }
            catch (Exception)
            {
                // Could not build a proper Uri. Abandon.
            }
        }

        private void BootStrapper_BackRequested(object sender, HandledEventArgs e)
        {
            if (webView.CanGoBack)
            {
                webView.GoBack();
                e.Handled = true;
            }
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
        }

    }
}
