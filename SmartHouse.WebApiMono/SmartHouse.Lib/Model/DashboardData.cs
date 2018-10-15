using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHouse.Lib
{
	public class DashboardData
	{
		public PandoraResult NowPlaying { get; set; }
		public string CurrentInput { get; set; }
		public TelemetryData TelemetryData { get; set; }
		public bool IsTurnOn { get; set; }
		public short Volume { get; set; }
	}
}
