using System;
namespace SmartHouse.Lib
{
	public class TemperatureData
	{
		public decimal Temperature { get; set; }
		public decimal Humidity { get; set; }
		public decimal HeatIndex { get; set; }
		public DateTime Measured { get; set; }
	}
}
