using System;
using System.Diagnostics;

namespace SmartHouse.Lib
{
	public static class BashHelper
	{
		public static void ExecBashCommand(string command)
		{
			using (var proc = new Process())
			{
				proc.StartInfo.FileName = "/bin/bash";
				proc.StartInfo.Arguments = "-c \" " + command + " \"";
				proc.StartInfo.UseShellExecute = false;
				proc.StartInfo.RedirectStandardInput = true;
				proc.StartInfo.RedirectStandardOutput = true;
				proc.Start();	

				proc.WaitForExit();
			}
		}

		public static void ExecBashCommandNoWait(string command)
		{
			using (var proc = new Process())
			{
				proc.StartInfo.FileName = "/bin/bash";
				proc.StartInfo.Arguments = "-c \" " + command + " \"";
				proc.StartInfo.UseShellExecute = false;
				proc.StartInfo.RedirectStandardInput = true;
				proc.StartInfo.RedirectStandardOutput = true;
				proc.Start();
			}
		}

		public static void ExecBashScript(string script)
		{
			using (var proc = new Process())
			{
				proc.StartInfo.FileName = "/bin/bash";
				proc.StartInfo.Arguments = script;
				proc.StartInfo.UseShellExecute = false;
				proc.StartInfo.RedirectStandardInput = true;
				proc.StartInfo.RedirectStandardOutput = true;
				proc.Start();

				proc.WaitForExit();
			}
		}

		public static void PlayAudio(string file)
		{
			var proc = new System.Diagnostics.Process();
			proc.EnableRaisingEvents = false;
			proc.StartInfo.FileName = "mplayer";
			proc.StartInfo.Arguments = $"{file}";
			proc.Start();

			proc.WaitForExit();
		}
	}
}
