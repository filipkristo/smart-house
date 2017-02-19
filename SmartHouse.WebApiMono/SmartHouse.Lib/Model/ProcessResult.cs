using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHouse.Lib
{
    public class ProcessResult
    {
        public string Message { get; set; }
        public string Error { get; set; }

        public override string ToString()
        {
            return $"Error: {Error}{Environment.NewLine}Output: {Message}";
        }
    }
}
