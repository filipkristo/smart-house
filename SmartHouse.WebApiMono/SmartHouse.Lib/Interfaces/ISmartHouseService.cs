using System;
using System.Threading.Tasks;

namespace SmartHouse.Lib
{
	public interface ISmartHouseService
	{
		Task<Result> SetMode(ModeEnum mode);
	}
}
