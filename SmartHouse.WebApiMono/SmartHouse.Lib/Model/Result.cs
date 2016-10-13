using System;
namespace SmartHouse.Lib
{
	public class Result
	{
		public string Message { get; set; }
		public bool Ok { get; set; } 
		public int ErrorCode { get; set; }

		public Result()
		{
		}
	}
}
