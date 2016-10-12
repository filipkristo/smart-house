using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.VoiceCommands;
using Windows.Storage;
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
            InitializeComponent();
        }

        public override async Task OnInitializeAsync(IActivatedEventArgs args)
        {
            try
            {
                Debug.WriteLine("Initialize Cortana file");

                var vcdStorageFile = await Package.Current.InstalledLocation.GetFileAsync(@"SmartHouseCommands.xml");
                await Windows.ApplicationModel.VoiceCommands.VoiceCommandDefinitionManager.InstallCommandDefinitionsFromStorageFileAsync(vcdStorageFile);

                Debug.WriteLine("Initialized Cortana file");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed initialized Cortana file: {ex.Message}");
            }

            await Task.CompletedTask;
        }        

        public override async Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
        {            
            NavigationService.Navigate(typeof(Views.MainPage));            
            await Task.CompletedTask;            
        }
        
    }
}

