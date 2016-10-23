using System;
using System.Runtime.Serialization;

namespace SmartHouse.AzureIot
{
	[DataContract]
	public class Command
	{
		[DataMember]
		public string Name;

		[DataMember]
		public CommandParameter[] Parameters;
	}
}
