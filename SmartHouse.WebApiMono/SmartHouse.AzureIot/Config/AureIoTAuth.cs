using System;
namespace SmartHouse.AzureIot
{
	public class AureIoTAuth
	{
		public static string DeviceId => "LivingRoom_Temp";
		public static string Hostname => "XXX";
		public static string DeviceKey => "XXX";

		public static string ConnectionString => $"HostName={Hostname};DeviceId={DeviceId};SharedAccessKey={DeviceKey}";
	}
}
