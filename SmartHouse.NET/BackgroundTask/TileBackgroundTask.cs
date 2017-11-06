using SmartHouse.LastFMApi;
using SmartHouse.LastFMApi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.ApplicationModel.Background;
using Windows.UI.Notifications;
using SmartHouse.UWPLib.Service;

namespace BackgroundTask
{
    public sealed class TileBackgroundTask : IBackgroundTask
    {
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            var deferral = taskInstance.GetDeferral();

            if (SettingsService.Instance.LiveTile)
            {
                using (var lastFmService = new LastFMService())
                {
                    await lastFmService.Login();
                    var artists = await lastFmService.RecentTopArtists();

                    if (artists.Any())
                        UpdateTile(artists);
                }

            }

            deferral.Complete();
        }

        private void UpdateTile(IEnumerable<ArtistTileData> artists)
        {
            // Create a tile update manager for the specified syndication feed.
            var updater = TileUpdateManager.CreateTileUpdaterForApplication();
            updater.EnableNotificationQueue(true);
            updater.Clear();

            // Keep track of the number feed items that get tile notifications.
            var itemCount = 0;

            // Create a tile notification for each feed item.
            foreach (var item in artists)
            {
                var tileXml = GetTileTemplate(item);

                // Create a new tile notification.
                var tileNotification = new TileNotification(tileXml)
                {
                    ExpirationTime = DateTime.Now.AddHours(2)
                };

                updater.Update(tileNotification);

                // Don't create more than 5 notifications.
                if (itemCount++ > 5)
                    break;
            }
        }

        private static Windows.Data.Xml.Dom.XmlDocument GetLargeTileTemplate(ArtistTileData item)
        {
            var tileXml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileWide310x150Text03);

            var srcAttribute = tileXml.CreateAttribute("src");
            srcAttribute.Value = item.Url;

            var altAttribute = tileXml.CreateAttribute("alt");
            altAttribute.Value = item.Name;

            var element = tileXml.GetElementsByTagName("image")[0];
            element.Attributes.SetNamedItem(srcAttribute);
            element.Attributes.SetNamedItem(altAttribute);

            return tileXml;
        }

        private Windows.Data.Xml.Dom.XmlDocument GetTileTemplate(ArtistTileData item)
        {
            var oldestScrobble = DateTime.Today.AddDays(-1);
            var hour = (int)(DateTime.Now - oldestScrobble).TotalHours;

            var xml = $@"<tile>
  <visual version=""2"">
    <binding template=""TileSquare150x150Image"" fallback=""TileSquareImage"">
      <image id=""1"" src=""{item.Url}"" alt=""{EscapeCharacters(item.Name)}""/>
    </binding>  
    <binding template=""TileWide310x150SmallImageAndText04"" fallback=""TileWideSmallImageAndText04"">
      <image id=""1"" src=""{item.Url}"" alt=""{EscapeCharacters(item.Name)}""/>
      <text id=""1"">{EscapeCharacters(item.Name)}</text>
      <text id=""2"">In last {hour} hours you have {item.Count} scrobble</text>
    </binding>
  </visual>
</tile>";

            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);

            return xmlDoc;
        }

        private string EscapeCharacters(string data)
        {
            var escapedString = data.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;").Replace("'", "&apos;");

            if (escapedString.Length > 17)
                escapedString = escapedString.Substring(0, 14) + "...";

            return escapedString;
        }
    }
}
