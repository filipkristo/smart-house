using System;
using System.Runtime.Serialization;

namespace SmartHouse.AzureIot
{
	[DataContract]
	public class Thermostat
	{
		[DataMember]
		public DeviceProperties DeviceProperties;

		[DataMember]
		public Command[] Commands;

		[DataMember]
		public bool IsSimulatedDevice;

		[DataMember]
		public string Version;

		[DataMember]
		public string ObjectType;
	}
}
