using Newtonsoft.Json;
using SmartHouse.Lib;
using SmartHouse.UWPLib.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SmartHouse.UWPLib.BLL
{
    public class SmartHouseService
    {
        public async Task<Result> Run(SmartHouseCommands command)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var uri = $"http://10.110.166.90:8081/api/SmartHouse/{command}";
                var json = await client.GetStringAsync(uri);

                return JsonConvert.DeserializeObject<Result>(json);
            }
        }

        public async Task<Result> RestartOpenVPN()
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var uri = $"http://10.110.166.90:8081/api/SmartHouse/RestartOpenVPN";
                var json = await client.GetStringAsync(uri);

                return JsonConvert.DeserializeObject<Result>(json);
            }
        }

        public async Task<Result> SetMode(string mode)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var uri = $"http://10.110.166.90:8081/api/SmartHouse/SetMode?Mode={mode}";
                var json = await client.GetStringAsync(uri);

                return JsonConvert.DeserializeObject<Result>(json);
            }
        }

        public async Task<Result> SetInput(string input)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var uri = $"http://10.110.166.90:8081/api/SmartHouse/{input}";
                var json = await client.GetStringAsync(uri);

                return JsonConvert.DeserializeObject<Result>(json);
            }
        }        

        public async Task<string> GetCurrentState()
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var uri = "http://10.110.166.90:8081/api/SmartHouse/GetCurrentState";
                var json = await client.GetStringAsync(uri);

                return JsonConvert.DeserializeObject<string>(json);
            }
        }
    }
}
