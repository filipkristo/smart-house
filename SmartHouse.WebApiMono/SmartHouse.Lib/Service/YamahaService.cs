using System;
using System.Threading.Tasks;

namespace SmartHouse.Lib
{
	public class YamahaService : IYamahaService
	{
		public YamahaService()
		{
			
		}

		public async Task<YamahaBasicStatus> GetInfo()
		{
			var xmlResponse = await YamahaHelper.DoRequest("<?xml version=\"1.0\" encoding=\"utf-8\"?><YAMAHA_AV cmd=\"GET\"><Main_Zone><Basic_Status>GetParam</Basic_Status></Main_Zone></YAMAHA_AV>");
			return YamahaHelper.SerializeXmlString<YamahaBasicStatus>(xmlResponse);
		}

		public async Task<string> TurnOn()
		{
			var xmlResponse = await YamahaHelper.DoRequest("<?xml version=\"1.0\" encoding=\"utf-8\"?><YAMAHA_AV cmd=\"PUT\"><Main_Zone><Power_Control><Power>On</Power></Power_Control></Main_Zone></YAMAHA_AV>");
			return xmlResponse;
		}

		public async Task<string> TurnOff()
		{
			var xmlResponse = await YamahaHelper.DoRequest("<?xml version=\"1.0\" encoding=\"utf-8\"?><YAMAHA_AV cmd=\"PUT\"><Main_Zone><Power_Control><Power>Standby</Power></Power_Control></Main_Zone></YAMAHA_AV>");
			return xmlResponse;
		}

		public async Task<string> SetInput(string input)
		{
			var xmlResponse = await YamahaHelper.DoRequest($"<?xml version=\"1.0\" encoding=\"utf-8\"?><YAMAHA_AV cmd=\"PUT\"><Main_Zone><Input><Input_Sel>{input}</Input_Sel></Input></Main_Zone></YAMAHA_AV>");
			return xmlResponse;
		}

		public async Task<string> VolumeUp()
		{
			var info = await GetInfo();
			var volume = info.Main_Zone.Basic_Status.Volume.Lvl.Val;

			var xmlResponse = await YamahaHelper.DoRequest($"<?xml version=\"1.0\" encoding=\"utf-8\"?><YAMAHA_AV cmd=\"PUT\"><Main_Zone><Volume><Lvl><Val>{volume + 5}</Val><Exp>1</Exp><Unit>dB</Unit></Lvl></Volume></Main_Zone></YAMAHA_AV>");
			return xmlResponse;
		}

		public async Task<string> VolumeDown()
		{
			var info = await GetInfo();
			var volume = info.Main_Zone.Basic_Status.Volume.Lvl.Val;

			var xmlResponse = await YamahaHelper.DoRequest($"<?xml version=\"1.0\" encoding=\"utf-8\"?><YAMAHA_AV cmd=\"PUT\"><Main_Zone><Volume><Lvl><Val>{volume - 5}</Val><Exp>1</Exp><Unit>dB</Unit></Lvl></Volume></Main_Zone></YAMAHA_AV>");
			return xmlResponse;
		}

		public async Task<PowerStatusEnum> PowerStatus()
		{
			var info = await GetInfo();
			var status = info.Main_Zone.Basic_Status.Power_Control.Power;

			if (status == "On")
				return PowerStatusEnum.On;
			else
				return PowerStatusEnum.StandBy;
		}
	}
}
