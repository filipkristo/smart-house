using System;
using System.Diagnostics;

namespace SmartHouse.Lib
{
    
	public static class BashHelper
	{
		public static ProcessResult ExecBashCommand(string command)
		{
			using (var proc = new Process())
			{
				proc.StartInfo.FileName = "/bin/bash";
				proc.StartInfo.Arguments = "-c \" " + command + " \"";
				proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardInput = true;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.RedirectStandardError = true;
                proc.Start();

                var output = proc.StandardOutput.ReadToEnd();
                var error = proc.StandardError.ReadToEnd();                

                proc.WaitForExit();

                return new ProcessResult()
                {
                    Error = error.Trim(),
                    Message = output.Trim()
                };
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

		public static void ExecBashScriptNoWait(string script)
		{
			using (var proc = new Process())
			{
				proc.StartInfo.FileName = "/bin/bash";
				proc.StartInfo.Arguments = script;
				proc.StartInfo.UseShellExecute = false;
				proc.StartInfo.RedirectStandardInput = true;
				proc.StartInfo.RedirectStandardOutput = true;
				proc.Start();
			}
		}


		public static void PlayAudio(string file)
		{
            using (var proc = new Process())
            {
                proc.EnableRaisingEvents = false;
                proc.StartInfo.FileName = "mplayer";
                proc.StartInfo.Arguments = $"{file}";
                proc.Start();

                proc.WaitForExit();
            }                
		}
	}
}
