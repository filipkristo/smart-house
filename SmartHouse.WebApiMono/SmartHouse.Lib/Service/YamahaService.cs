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

        public async Task<short> GetVolume()
        {
            var info = await GetInfo();
            return info.Main_Zone.Basic_Status.Volume.Lvl.Val;
        }

		public async Task<int> VolumeUp()
		{			
			var volume = await GetVolume();
            var newVolume = volume + 5;

			var xmlResponse = await YamahaHelper.DoRequest($"<?xml version=\"1.0\" encoding=\"utf-8\"?><YAMAHA_AV cmd=\"PUT\"><Main_Zone><Volume><Lvl><Val>{newVolume}</Val><Exp>1</Exp><Unit>dB</Unit></Lvl></Volume></Main_Zone></YAMAHA_AV>");
			return newVolume;
		}

		public async Task<int> VolumeDown()
		{
            var volume = await GetVolume();
            var newVolume = volume - 5;

            var xmlResponse = await YamahaHelper.DoRequest($"<?xml version=\"1.0\" encoding=\"utf-8\"?><YAMAHA_AV cmd=\"PUT\"><Main_Zone><Volume><Lvl><Val>{newVolume}</Val><Exp>1</Exp><Unit>dB</Unit></Lvl></Volume></Main_Zone></YAMAHA_AV>");
			return newVolume;
		}

		public async Task<string> SetVolume(int volume)
		{
			var xmlResponse = await YamahaHelper.DoRequest($"<?xml version=\"1.0\" encoding=\"utf-8\"?><YAMAHA_AV cmd=\"PUT\"><Main_Zone><Volume><Lvl><Val>{volume}</Val><Exp>1</Exp><Unit>dB</Unit></Lvl></Volume></Main_Zone></YAMAHA_AV>");
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
