using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SmartHouse.Lib
{
    public class LyricsService : ILyricsService
    {
        public async Task<string> GetMetroLyrics(string artist, string song)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.UserAgent.ParseAdd("Smart House/1.0");

                var parsedArtist = artist.ToLower().Trim().Replace(" ", "-");
                var parsedSong = song.ToLower().Trim().Replace(" ", "-");

                var uri = $"http://www.metrolyrics.com/{parsedSong}-lyrics-{parsedArtist}.html";
                var html = await client.GetStringAsync(uri);

                var index = html.IndexOf("lyrics-body-text") + "lyrics-body-text".Length + "  class=  js-lyric-text >".Length;
                html = html.Substring(index);
                index = html.IndexOf("writers");

                return html.Substring(0, index - "< p class=  </div> </div>".Length);
            }
        }
    }
}
