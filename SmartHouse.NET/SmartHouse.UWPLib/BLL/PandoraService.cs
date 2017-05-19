using Newtonsoft.Json;
using SmartHouse.Lib;
using SmartHouse.UWPLib.Model;
using SmartHouse.UWPLib.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SmartHouse.UWPLib.BLL
{
    public class PandoraService
    {
        private readonly SettingsService settingsService;

        public PandoraService()
        {
            settingsService = SettingsService.Instance;
        }

        public async Task<Result> Run(PandoraCommands command)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var uri = $"http://{settingsService.HostIP}:{settingsService.HostPort}/api/Pandora/{command}";
                var json = await client.GetStringAsync(uri);

                return JsonConvert.DeserializeObject<Result>(json);
            }
        }

        public async Task<SongResult> ShowInfo()
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var uri = $"http://{settingsService.HostIP}:{settingsService.HostPort}/api/SmartHouse/NowPlaying";
                var json = await client.GetStringAsync(uri);

                return JsonConvert.DeserializeObject<SongResult>(json);
            }
        }

        public async Task<Result> NextStation()
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var uri = $"http://{settingsService.HostIP}:{settingsService.HostPort}/api/Pandora/NextStation";
                var json = await client.GetStringAsync(uri);

                return JsonConvert.DeserializeObject<Result>(json);
            }
        }

        public async Task<Result> PrevStation()
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var uri = $"http://{settingsService.HostIP}:{settingsService.HostPort}/api/Pandora/PrevStation";
                var json = await client.GetStringAsync(uri);

                return JsonConvert.DeserializeObject<Result>(json);
            }
        }
    }
}
