using System;
using System.Threading.Tasks;

namespace SmartHouse.Lib
{
	public interface ISmartHouseService
	{
		Task<Result> RestartOpenVPNService();

		Task<Result> RestartOpenVPNServiceTcp();

		Task<Result> SetMode(ModeEnum mode);

		Task SaveState(SmartHouseState state);

		Task<SmartHouseState> GetCurrentState();

		Result PlayAlarm();

		Task<Result> PlayAlarmTcp();
	}
}
