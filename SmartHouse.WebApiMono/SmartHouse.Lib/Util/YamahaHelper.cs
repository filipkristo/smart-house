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

		public async static Task<string> DoRequest(string command)
		{
            const string uri = "http://10.110.167.49/YamahaRemoteControl/ctrl";

			using (var client = new HttpClient())
			{	
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
				client.DefaultRequestHeaders.Add("User-Agent", "SmartHouse");

				var payload = new StringContent(command, Encoding.Default, "application/xml");
				var result = await client.PostAsync(uri, payload);

				if (!result.IsSuccessStatusCode)
					throw new Exception($"Error while execuring command. ErrorCode: {result.StatusCode}");

				var bytes = await result.Content.ReadAsByteArrayAsync();
				return Encoding.UTF8.GetString(bytes);
			}
		}

		public static T SerializeXmlString<T>(string xml) where T : new()
		{
			return XmlConvert.DeserializeObject<T>(xml);
		}
	}
}
