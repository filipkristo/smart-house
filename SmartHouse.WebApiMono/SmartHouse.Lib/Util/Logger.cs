using System;
namespace SmartHouse.Lib
{
	public static class Logger
	{
		private static log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		public static void LogInfoMessage(String Message)
		{
			lock (Log)
			{
				Log.Info(Message);
			}
		}

		public static void LogDebugMessage(String Message)
		{
			lock (Log)
			{
				Log.Debug(Message);
			}
		}

		public static void LogConsole(String Message)
		{
			lock (Log)
			{
				Log.Debug(Message);
			}
		}

		public static void LogErrorMessage(String Message, Exception ex)
		{
			lock (Log)
			{
				Log.Error(Message, ex);
			}
		}
	}
}
