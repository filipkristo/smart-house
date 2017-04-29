using System;
using System.Threading.Tasks;

namespace SmartHouse.Lib
{
	public interface ITelemetryService : IDisposable
	{
		Result SaveTemperature(TemperatureData data);
		Task<Result> SaveTemperatureUdp(string data);
        Task<TemperatureData> GetLastTemperature();
        Task<Result> AirCondition(byte On);
    }
}
