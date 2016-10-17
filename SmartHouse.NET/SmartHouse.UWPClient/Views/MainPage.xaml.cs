using System;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace SmartHouse.UWPClient.Views
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;

            webView.PermissionRequested += webView_PermissionRequested;
            webView.ContainsFullScreenElementChanged += webView_ContainsFullScreenElementChanged;
            webView.NavigationCompleted += webView1_NavigationCompleted;                                  
        }

        private void webView_PermissionRequested(WebView sender, WebViewPermissionRequestedEventArgs args)
        {
            if (args.PermissionRequest.PermissionType == WebViewPermissionType.Geolocation)
            {
                args.PermissionRequest.Allow();
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            webView.Navigate(new Uri("http://10.110.166.90:8081/swagger"));
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

        private void webView1_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            if (args.IsSuccess == true)
            {
                //statusTextBlock.Text = "Navigation to " + args.Uri.ToString() + " completed successfully.";
            }
            else
            {
                //statusTextBlock.Text = "Navigation to: " + args.Uri.ToString() +
                //                       " failed with error " + args.WebErrorStatus.ToString();
            }
        }
    }
}
