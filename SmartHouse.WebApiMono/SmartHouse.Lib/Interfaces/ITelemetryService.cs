using System;
using System.Threading.Tasks;

namespace SmartHouse.Lib
{
	public interface ITelemetryService : IDisposable
	{
		Task<Result> SaveTemperature(TemperatureData data);
		Task<Result> SaveTemperatureUdp(string data);
		Task<TemperatureData> GetLastTemperature();
	}
}
