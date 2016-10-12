using SmartHouse.UWPLib.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SmartHouse.UWPLib.BLL
{
    public class PandoraCommand
    {
        public async Task Run(Commands command)
        {
            using (var client = new HttpClient())
            {
                var uri = $"http://10.110.166.90/player/pandoraApi.php?q={command.ToString()}";
                var json = await client.GetStringAsync(uri);
            }
        }
    }
}
