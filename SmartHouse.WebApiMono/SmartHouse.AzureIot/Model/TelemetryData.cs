using System;
using System.Runtime.Serialization;

namespace SmartHouse.AzureIot
{
	[DataContract]
	public class TelemetryData
	{
		[DataMember]
		public string DeviceId;

		[DataMember]
		public decimal Temperature;

		[DataMember]
		public decimal Humidity;

		[DataMember]
		public decimal HeatIndex;

	}

}
