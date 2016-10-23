using System;
using System.Runtime.Serialization;

namespace SmartHouse.AzureIot
{
	[DataContract]
	public class DeviceProperties
	{
		[DataMember]
		public string DeviceID;

		[DataMember]
		public bool HubEnabledState;

		[DataMember]
		public string CreatedTime;

		[DataMember]
		public string DeviceState;

		[DataMember]
		public string UpdatedTime;

		[DataMember]
		public string Manufacturer;

		[DataMember]
		public string ModelNumber;

		[DataMember]
		public string SerialNumber;

		[DataMember]
		public string FirmwareVersion;

		[DataMember]
		public string Platform;

		[DataMember]
		public string Processor;

		[DataMember]
		public string InstalledRAM;

		[DataMember]
		public double Latitude;

		[DataMember]
		public double Longitude;

	}
}
