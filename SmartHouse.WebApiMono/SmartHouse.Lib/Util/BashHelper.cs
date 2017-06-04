using System;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace SmartHouse.Lib
{
    
	public static class BashHelper
	{
        /// <summary>
        /// Quotes all arguments that contain whitespace, or begin with a quote and returns a single
        /// argument string for use with Process.Start().
        /// </summary>
        /// <param name="args">A list of strings for arguments, may not contain null, '\0', '\r', or '\n'</param>
        /// <returns>The combined list of escaped/quoted strings</returns>
        /// <exception cref="System.ArgumentNullException">Raised when one of the arguments is null</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Raised if an argument contains '\0', '\r', or '\n'</exception>
        public static string EscapeArguments(params string[] args)
        {
            var arguments = new StringBuilder();
            var invalidChar = new Regex("[\x00\x0a\x0d]");//  these can not be escaped
            var needsQuotes = new Regex(@"\s|""");//          contains whitespace or two quote characters
            var escapeQuote = new Regex(@"(\\*)(""|$)");//    one or more '\' followed with a quote or end of string

            for (int carg = 0; args != null && carg < args.Length; carg++)
            {
                if (args[carg] == null) { throw new ArgumentNullException("args[" + carg + "]"); }
                if (invalidChar.IsMatch(args[carg])) { throw new ArgumentOutOfRangeException("args[" + carg + "]"); }
                if (args[carg] == String.Empty) { arguments.Append("\"\""); }
                else if (!needsQuotes.IsMatch(args[carg])) { arguments.Append(args[carg]); }
                else
                {
                    arguments.Append('"');
                    arguments.Append(escapeQuote.Replace(args[carg], m =>
                    m.Groups[1].Value + m.Groups[1].Value +
                    (m.Groups[2].Value == "\"" ? "\\\"" : "")
                    ));
                    arguments.Append('"');
                }
                if (carg + 1 < args.Length)
                    arguments.Append(' ');
            }
            return arguments.ToString();
        }

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

		public static ProcessResult ExecBashScript(string script, params string[] args)
		{
			using (var proc = new Process())
			{
				proc.StartInfo.FileName = "/bin/bash";
				proc.StartInfo.Arguments = script;
				proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardInput = true;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.RedirectStandardError = true;
                proc.Start();

                if(args != null && args.Length > 0)
                {
                    using (var stream = proc.StandardInput)
                    {
                        foreach (var item in args)
                        {
                            stream.WriteLine(item);
                        }
                    }                        
                }                

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
