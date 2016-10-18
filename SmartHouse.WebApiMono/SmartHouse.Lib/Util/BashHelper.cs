using System;
using System.Diagnostics;

namespace SmartHouse.Lib
{
	public static class BashHelper
	{
		public static void ExecBashCommand(string command)
		{
			var proc = new Process();

			proc.StartInfo.FileName = "/bin/bash";
			proc.StartInfo.Arguments = "-c \" " + command + " \"";
			proc.StartInfo.UseShellExecute = false;
			proc.StartInfo.RedirectStandardInput = true;
			proc.StartInfo.RedirectStandardOutput = true;
			proc.Start();
		}

		public static void ExecBashScript(string script)
		{
			var proc = new Process();

			proc.StartInfo.FileName = "/bin/bash";
			proc.StartInfo.Arguments = script;
			proc.StartInfo.UseShellExecute = false;
			proc.StartInfo.RedirectStandardInput = true;
			proc.StartInfo.RedirectStandardOutput = true;
			proc.Start();
		}
	}
}
