using System;
namespace SmartHouse.Lib
{
	public class TcpCommands
	{
		public class Pandora 
		{
			public const string PANDORA_START = "Pandora start";
			public const string PANDORA_STOP = "Pandora stop";
			public const string PANDORA_RESTART = "Pandora restart";
		}

		public class SmartHouse
		{
			public const string RESTART_VPN = "Smart House - Restart VPN"; 
		}

		public Result ExecuteTcpCommand(string command)
		{
			switch (command)
			{
				case Pandora.PANDORA_START:
					return new PandoraService().Start();
				case Pandora.PANDORA_STOP:
					return new PandoraService().Stop();
				case Pandora.PANDORA_RESTART:
					return new PandoraService().Restart();
				case SmartHouse.RESTART_VPN:
					return new SmartHouseService().RestartOpenVPNService().Result;
				default:
					throw new Exception($"Command {command} is not defined");
			}
		}
	}
}
