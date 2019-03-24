using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Xml.Net;

namespace SmartHouse.Lib
{
	public static class YamahaHelper
	{
        private static readonly HttpClient client = new HttpClient();

        static YamahaHelper()
        {
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
            client.DefaultRequestHeaders.Add("User-Agent", "smart-house");
        }

		public async static Task<string> DoRequest(string command)
		{
            const string uri = "http://10.110.167.49/YamahaRemoteControl/ctrl";

            var payload = new StringContent(command, Encoding.Default, "application/xml");
            var result = await client.PostAsync(uri, payload).ConfigureAwait(false);

            if (!result.IsSuccessStatusCode)
                throw new Exception($"Error while execuring command. ErrorCode: {result.StatusCode}");

            var bytes = await result.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
            return Encoding.UTF8.GetString(bytes);
        }

		public static T SerializeXmlString<T>(string xml) where T : new()
		{
			return XmlConvert.DeserializeObject<T>(xml);
		}
	}
}
