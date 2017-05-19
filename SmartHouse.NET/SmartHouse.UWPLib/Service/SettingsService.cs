using System;
using Template10.Common;
using Template10.Utils;
using Windows.Security.Credentials;
using Windows.System.Profile;
using Windows.UI.Xaml;

namespace SmartHouse.UWPLib.Service
{
    public class SettingsService
    {
        private const string WEB_APP = "SmartHouseWeb";        

        public static SettingsService Instance { get; } = new SettingsService();
        Template10.Services.SettingsService.ISettingsHelper _helper;        

        private SettingsService()
        {
            _helper = new Template10.Services.SettingsService.SettingsHelper();            
        }

        public bool UseShellBackButton
        {
            get { return _helper.Read<bool>(nameof(UseShellBackButton), true); }
            set
            {
                _helper.Write(nameof(UseShellBackButton), value);
                BootStrapper.Current.NavigationService.Dispatcher.Dispatch(() =>
                {
                    BootStrapper.Current.ShowShellBackButton = value;
                    BootStrapper.Current.UpdateShellBackButton();
                    BootStrapper.Current.NavigationService.Refresh();
                });
            }
        }

        public bool UseBackgroundWorker
        {
            get { return _helper.Read<bool>(nameof(UseBackgroundWorker), false); }
            set
            {
                _helper.Write(nameof(UseBackgroundWorker), value);                
            }
        }

        public ApplicationTheme AppTheme
        {
            get
            {
                var theme = ApplicationTheme.Light;
                var value = _helper.Read<string>(nameof(AppTheme), theme.ToString(), Template10.Services.SettingsService.SettingsStrategies.Roam);
                return Enum.TryParse<ApplicationTheme>(value, out theme) ? theme : ApplicationTheme.Dark;
            }
            set
            {
                _helper.Write(nameof(AppTheme), value.ToString(), Template10.Services.SettingsService.SettingsStrategies.Roam);                
            }
        }

        public TimeSpan CacheMaxDuration
        {
            get { return _helper.Read<TimeSpan>(nameof(CacheMaxDuration), TimeSpan.FromDays(2)); }
            set
            {
                _helper.Write(nameof(CacheMaxDuration), value);
                BootStrapper.Current.CacheMaxDuration = value;
            }
        }

        public string WebHost
        {
            get { return _helper.Read<string>(nameof(WebHost), "", Template10.Services.SettingsService.SettingsStrategies.Roam); }
            set
            {
                _helper.Write(nameof(WebHost), value, Template10.Services.SettingsService.SettingsStrategies.Roam);
            }
        }

        public string HostIP
        {
            get { return _helper.Read<string>(nameof(HostIP), "", Template10.Services.SettingsService.SettingsStrategies.Roam); }
            set
            {
                _helper.Write(nameof(HostIP), value, Template10.Services.SettingsService.SettingsStrategies.Roam);
            }
        }

        public string HostPort
        {
            get { return _helper.Read<string>(nameof(HostPort), "", Template10.Services.SettingsService.SettingsStrategies.Roam); }
            set
            {
                _helper.Write(nameof(HostPort), value, Template10.Services.SettingsService.SettingsStrategies.Roam);
            }
        }        

        public PasswordCredential GetCredentialFromLocker()
        {
            PasswordCredential credential = null;

            try
            {
                var vault = new PasswordVault();
                var credentialList = vault.FindAllByResource(WEB_APP);

                if (credentialList.Count > 0)
                {
                    if (credentialList.Count == 1)
                    {
                        credential = credentialList[0];
                        credential.RetrievePassword();
                    }
                }
            }
            catch { }

            return credential;
        }

        public void SaveUsernamePassword(string username, string password)
        {
            var vault = new PasswordVault();                                
            vault.Add(new PasswordCredential(WEB_APP, username, password));
        }

        public void DeviceInformation()
        {
            // get the system family information
            var deviceFamily = AnalyticsInfo.VersionInfo.DeviceFamily;

            // get the system version number
            var deviceFamilyVersion = AnalyticsInfo.VersionInfo.DeviceFamilyVersion;

            var version = ulong.Parse(deviceFamilyVersion);
            var majorVersion = (version & 0xFFFF000000000000L) >> 48;
            var minorVersion = (version & 0x0000FFFF00000000L) >> 32;
            var buildVersion = (version & 0x00000000FFFF0000L) >> 16;
            var revisionVersion = (version & 0x000000000000FFFFL);
            var systemVersion = $"{majorVersion}.{minorVersion}.{buildVersion}.{revisionVersion}";
        }
    }
}
