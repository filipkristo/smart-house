using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace SmartHouse.UWPClient.ViewModels
{
    public class BaseWebViewViewModel : BaseViewModel
    {
        public string ItemUrl { get { return Get<string>(); } set { Set(value); } }
        public string PageUrl { get; private set; }
        public WebView WebView { get; private set; }

        public BaseWebViewViewModel(string Url)
        {
            PageUrl = Url;
        }

        public override Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            if (string.IsNullOrWhiteSpace(ItemUrl))
                Refresh();

            return Task.CompletedTask;
        }

        public void webView_ContainsFullScreenElementChanged(WebView sender, object args)
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

        public void webView_PermissionRequested(WebView sender, WebViewPermissionRequestedEventArgs args)
        {
            if (args.PermissionRequest.PermissionType == WebViewPermissionType.Geolocation)
            {
                args.PermissionRequest.Allow();
            }
        }

        public void webView_Loaded(object sender, RoutedEventArgs e)
        {
            WebView = sender as WebView;
        }

        public virtual void Refresh()
        {
            ItemUrl = null;
            ItemUrl = PageUrl;
        }
    }
}
