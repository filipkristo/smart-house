﻿using System;
using System.Threading.Tasks;

namespace SmartHouse.Lib
{
	public interface ITelemetryService : IDisposable
	{
        Action<TelemetryData> SignalR { get; set; }
        Result SaveTemperature(TelemetryData data);
		Task<Result> SaveTemperatureUdp(string data);
        Task<TelemetryData> GetLastTemperature();
        Task<byte> GetAirConditionState();
        Task<Result> AirCondition(byte On);
    }
}
