using System;
using System.Threading.Tasks;

namespace SmartHouse.Lib
{
	public interface IYamahaService
	{
		Task<YamahaBasicStatus> GetInfo();

		Task<string> TurnOn();

		Task<string> TurnOff();

		Task<string> SetInput(string input);

		Task<string> VolumeUp();

		Task<string> VolumeDown();

		Task<string> SetVolume(int volume);

		Task<PowerStatusEnum> PowerStatus();
	}
}
