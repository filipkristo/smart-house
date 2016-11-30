using System;
namespace SmartHouse.Lib
{
	public class KeyValue : IEquatable<KeyValue>
	{
		public string Key { get; set; }
		public string Value { get; set; }

		public KeyValue()
		{
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as KeyValue);
		}

		public override int GetHashCode()
		{
			return Key.GetHashCode() + Value.GetHashCode();
		}

		public override string ToString()
		{
			return string.Format("[KeyValue: Key={0}, Value={1}]", Key, Value);
		}

		public bool Equals(KeyValue other)
		{
			return other?.Key == Key && other?.Value == Value;
		}
	}
}
