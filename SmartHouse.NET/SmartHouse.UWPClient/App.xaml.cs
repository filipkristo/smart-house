using Microsoft.HockeyApp;
using SmartHouse.UWPClient.Services.SettingsServices;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Template10.Controls;
using Template10.Utils;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.VoiceCommands;
using Windows.Foundation.Metadata;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace SmartHouse.UWPClient
{
    /// Documentation on APIs used in this page:
    /// https://github.com/Windows-XAML/Template10/wiki
    [Bindable]
    sealed partial class App : Template10.Common.BootStrapper
    {
        public App()
        {
            Microsoft.HockeyApp.HockeyClient.Current.Configure("c7217e3f12be43108f4de5b1c9fdd02a");
            InitializeComponent();
        }

        public override async Task OnInitializeAsync(IActivatedEventArgs args)
        {
            if (Window.Current.Content as ModalDialog == null)
            {
                // create a new frame 
                var nav = NavigationServiceFactory(BackButton.Attach, ExistingContent.Include);
                // create modal root
                Window.Current.Content = new ModalDialog{DisableBackButtonWhenModal = true, Content = new Views.Shell(nav), ModalContent = new Views.Busy()};
            }

            (Window.Current.Content as FrameworkElement).RequestedTheme = SettingsService.Instance.AppTheme.ToElementTheme();
            Views.Shell.HamburgerMenu.RefreshStyles(SettingsService.Instance.AppTheme);
        }

        public override async Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
        {
            NavigationService.Navigate(typeof (Views.MainPage));
            await Task.CompletedTask;
        }
    }
}