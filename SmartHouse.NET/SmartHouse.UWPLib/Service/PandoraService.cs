using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SmartHouse.Lib;
using SmartHouse.UWPLib.Model;

namespace SmartHouse.UWPLib.Service
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
