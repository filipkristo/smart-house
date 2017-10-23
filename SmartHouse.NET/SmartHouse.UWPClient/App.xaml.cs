using Microsoft.AspNet.SignalR.Client;
using Microsoft.HockeyApp;
using SmartHouse.Lib;
using SmartHouse.UWPClient.Messaging;
using SmartHouse.UWPLib.Service;
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
        private HubConnection hubConnection = null;

        public App()
        {
            HockeyClient.Current.Configure("c7217e3f12be43108f4de5b1c9fdd02a");
            InitializeComponent();

            ShowShellBackButton = SettingsService.Instance.UseShellBackButton;
            CacheMaxDuration = SettingsService.Instance.CacheMaxDuration;
            ShowShellBackButton = SettingsService.Instance.UseShellBackButton;
            AutoSuspendAllFrames = true;
            AutoRestoreAfterTerminated = true;
            AutoExtendExecutionSession = true;
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

            if (ApiInformation.IsTypePresent("Windows.ApplicationModel.VoiceCommands.VoiceCommandDefinitionManager"))
            {
                try
                {
                    Debug.WriteLine("Initialize Cortana file");
                    var vcdStorageFile = await Package.Current.InstalledLocation.GetFileAsync(@"SmartHouseCommands.xml");
                    await VoiceCommandDefinitionManager.InstallCommandDefinitionsFromStorageFileAsync(vcdStorageFile);
                    Debug.WriteLine("Initialized Cortana file");
                }
                catch (Exception ex)
                {
                    Microsoft.HockeyApp.HockeyClient.Current.TrackException(ex);
                    Debug.WriteLine($"Failed initialized Cortana file: {ex.Message}");
                }
            }

            if (SettingsService.Instance.UseBackgroundWorker)
            {
                await StartSignalRClient();
                var task = StartAzureIotMessaging();                
            }                
        }

        private async Task StartAzureIotMessaging()
        {
            var messaging = new AzureIotMessaging();
            await messaging.ReceiveDataFromAzure();
        }

        private async Task StartSignalRClient()
        {
            var IP = SettingsService.Instance.HostIP;
            var Port = SettingsService.Instance.HostPort;

            hubConnection = new HubConnection($"http://{IP}:{Port}/");
            var hubProxy = hubConnection.CreateHubProxy("ServerHub");
            hubProxy.On<TelemetryData>("temperature", async (data) => await UploadToCloud(data));

            await hubConnection.Start();
        }

        int skiped;
        private async Task UploadToCloud(TelemetryData telemetry)
        {
            if(skiped == 6)
            {
                skiped = 0;

                try
                {
                    var settings = SettingsService.Instance;
                    var credential = settings.GetCredentialFromLocker();

                    if (!string.IsNullOrWhiteSpace(settings.WebHost) && credential != null)
                    {
                        var webclient = new WebClientService(settings.WebHost, credential.UserName, credential.Password);

                        await webclient.Login();
                        var result = await webclient.SendTelemetryData(telemetry);

                        Debug.WriteLine($"Result: {result}");
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }                
            }            
            else
            {
                skiped++;
            }
        }

        public override async Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
        {
            NavigationService.Navigate(typeof(Views.MainPage));
            await Task.CompletedTask;
        }

        public override Task OnSuspendingAsync(object s, SuspendingEventArgs e, bool prelaunchActivated)
        {            
            return base.OnSuspendingAsync(s, e, prelaunchActivated);
        }


    }
}