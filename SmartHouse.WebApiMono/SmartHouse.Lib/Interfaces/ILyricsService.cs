using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHouse.Lib
{
    public interface ILyricsService
    {
        Task<string> GetMetroLyrics(string artist, string song);
    }
}
