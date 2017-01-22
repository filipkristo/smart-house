using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartHouse.Lib;
using System.Web.Http;

namespace SmartHouse.WebApiMono.Controllers
{
    [RoutePrefix("api/Lyrics")]
    public class LyricsController : BaseController
    {
        private readonly ILyricsService LyricsService;

        public LyricsController(ISettingsService service, ILyricsService lyricsService) : base(service)
        {
            LyricsService = lyricsService;
        }

        [Route("GetMetroLyrics")]
        [HttpGet]
        public async Task<string> GetMetroLyrics(string artist, string song)
        {
            return await LyricsService.GetMetroLyrics(artist, song);
        }
    }
}
