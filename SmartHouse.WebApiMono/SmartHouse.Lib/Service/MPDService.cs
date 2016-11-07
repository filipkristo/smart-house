using System;
using System.Net;
using Libmpc;

namespace SmartHouse.Lib
{
	public class MPDService : IMPDService
	{
		private readonly Mpc MpdClient = new Mpc();

		public MPDService()
		{
			MpdClient.Connection = new MpcConnection(new System.Net.IPEndPoint(IPAddress.Parse("10.110.166.90"), 6600));
		}

		public Result Play()
		{
			MpdClient.Play();

			return new Result()
			{
				ErrorCode = 0,
				Message = "Ok",
				Ok = true
			};
		}

		public Result Pause()
		{
			MpdClient.Pause(true);

			return new Result()
			{
				ErrorCode = 0,
				Message = "Ok",
				Ok = true
			};
		}

		public Result Stop()
		{
			MpdClient.Stop();

			return new Result()
			{
				ErrorCode = 0,
				Message = "Ok",
				Ok = true
			};
		}

		public Result Next()
		{
			MpdClient.Next();

			return new Result()
			{
				ErrorCode = 0,
				Message = "Ok",
				Ok = true
			};
		}

		public Result Previous()
		{
			MpdClient.Previous();

			return new Result()
			{
				ErrorCode = 0,
				Message = "Ok",
				Ok = true
			};
		}

		public MpdStatus GetStatus()
		{
			return MpdClient.Status();
		}

		public void Dispose()
		{
			MpdClient?.Connection.Disconnect();
		}
	}
}
