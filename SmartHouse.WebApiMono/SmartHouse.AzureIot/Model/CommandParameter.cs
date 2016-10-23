using System;
using System.Runtime.Serialization;

namespace SmartHouse.AzureIot
{
	[DataContract]
	public class CommandParameter
	{
		[DataMember]
		public string Name;

		[DataMember]
		public string Type;
	}
}
