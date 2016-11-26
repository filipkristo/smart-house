using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHouse.Lib
{
	public class PandoraService : IPanodraService
	{
		public PandoraService()
		{
		}

		public Result Play()
		{
			return CommandExecuter(PandoraCommandEnum.Play);
		}

		public Result Pause()
		{
			return CommandExecuter(PandoraCommandEnum.Pause);
		}

		public async Task<Result> Start()
		{
			var result = CommandExecuter(PandoraCommandEnum.Start);
			await Task.Delay(TimeSpan.FromSeconds(4));

			return result;
		}

		public Result Stop()
		{
			return CommandExecuter(PandoraCommandEnum.Stop);
		}

		public async Task<Result> Restart()
		{
			var result = Stop();
			await Task.Delay(TimeSpan.FromSeconds(1));

			await Start();
			return result;
		}

		public async Task<Result> StartTcp()
		{
			var tcp = new TcpServer();
			return await tcp.SendCommandToServer<Result>(TcpCommands.Pandora.PANDORA_START);
		}

		public async Task<Result> StopTcp()
		{
			var tcp = new TcpServer();
			return await tcp.SendCommandToServer<Result>(TcpCommands.Pandora.PANDORA_STOP);
		}

		public async Task<Result> RestartTcp()
		{
			var tcp = new TcpServer();
			return await tcp.SendCommandToServer<Result>(TcpCommands.Pandora.PANDORA_RESTART);
		}

		public Result Next()
		{
			return CommandExecuter(PandoraCommandEnum.Next);
		}

		public Result ThumbUp()
		{
			return CommandExecuter(PandoraCommandEnum.ThumbUp);
		}

		public Result ThumbDown()
		{
			return CommandExecuter(PandoraCommandEnum.ThumbDown);
		}

		public Result TiredOfThisSong()
		{
			return CommandExecuter(PandoraCommandEnum.Tired);
		}

		public Result VolumeUp()
		{
			return CommandExecuter(PandoraCommandEnum.VolumeUp);
		}

		public Result VolumeDown()
		{
			return CommandExecuter(PandoraCommandEnum.VolumeDown);
		}

		public Result ChangeStation(string stationId)
		{
			BashHelper.ExecBashCommand($"echo 's{stationId}' >> /home/pi/.config/pianobar/ctl");

			return new Result()
			{
				ErrorCode = 0,
				Message = $"Changed station {stationId}",
				Ok = true
			};
		}

		public Result NextStation()
		{
			var info = GetCurrentSongInfo();
			var stations = GetStationList().ToList();

			var currentStation = stations.FirstOrDefault(x => x.Value.Contains(info.Radio));
			var currentStationIndex = stations.IndexOf(currentStation);

			var nextStation = default(KeyValue);

			if (currentStationIndex == stations.Count - 1)
				currentStation = stations.First();
			else
				currentStation = stations[currentStationIndex + 1];

			ChangeStation(nextStation.Key);

			return new Result()
			{
				ErrorCode = 0,
				Message = $"Starting to play {nextStation.Value} station",
				Ok = true
			};
		}

		public Result PrevStation()
		{
			var info = GetCurrentSongInfo();
			var stations = GetStationList().ToList();

			var currentStation = stations.FirstOrDefault(x => x.Value.Contains(info.Radio));
			var currentStationIndex = stations.IndexOf(currentStation);

			var nextStation = default(KeyValue);

			if (currentStationIndex == 0)
				currentStation = stations.Last();
			else
				currentStation = stations[currentStationIndex - 1];

			ChangeStation(nextStation.Key);

			return new Result()
			{
				ErrorCode = 0,
				Message = $"Starting to play {nextStation.Value} station",
				Ok = true
			};
		}

		public PandoraResult GetCurrentSongInfo()
		{
			var homeDir = System.Environment.GetEnvironmentVariable("HOME");
			var path = Path.Combine(homeDir, @".config/pianobar/nowplaying");

			var lines = File.ReadLines(path).Select(x => x?.Trim()).ToList();

			var fileInfo = new FileInfo(path);
			var stamp = DateTime.Now - fileInfo.LastWriteTime;

			return new PandoraResult()
			{
				Artist = lines[0],
				Song = lines[1],
				Radio = lines[2],
				Loved = lines[3] == "1",
				AlbumUri = lines[4],
				Album = lines[5],
				DurationSeconds = Convert.ToInt32(lines[6]),
				IsPlaying = stamp.TotalSeconds <= Convert.ToInt32(lines[6]),
				LastModifed = fileInfo.LastWriteTime
			};
		}

		public IEnumerable<KeyValue> GetStationList()
		{
			var homeDir = System.Environment.GetEnvironmentVariable("HOME");
			var path = Path.Combine(homeDir, @".config/pianobar/stationlist");

			var lines = File.ReadLines(path)						
						.Select(x => new KeyValue
						{
							Key = x.Replace(")", "").Substring(0, x.Replace(")", "").IndexOf(' ')),
							Value = x.Replace(")", "")
						}).ToList();
			
			return lines;
		}

		public bool IsPlaying()
		{
			return GetCurrentSongInfo().IsPlaying;
		}

		private Result CommandExecuter(PandoraCommandEnum command)
		{
			var message = String.Empty;
			switch (command)
			{
				case PandoraCommandEnum.Play:
					BashHelper.ExecBashCommand("echo 'p' >> /home/pi/.config/pianobar/ctl");
					message = "Play/Pause";
					break;
				case PandoraCommandEnum.Pause:
					BashHelper.ExecBashCommand("echo 'p' >> /home/pi/.config/pianobar/ctl");
					message = "Play/Pause";
					break;
				case PandoraCommandEnum.Start:
					BashHelper.ExecBashScriptNoWait("./pandora.sh start");
					message = "Start";
					break;
				case PandoraCommandEnum.Stop:
					BashHelper.ExecBashScriptNoWait("./pandora.sh stop");
					message = "Stop";
					break;				
				case PandoraCommandEnum.Next:
					BashHelper.ExecBashCommand("echo 'n' >> /home/pi/.config/pianobar/ctl");
					message = "Next";
					break;
				case PandoraCommandEnum.ThumbUp:
					BashHelper.ExecBashCommand("echo '+' >> /home/pi/.config/pianobar/ctl");
					message = "Thumb Up";
					break;
				case PandoraCommandEnum.ThumbDown:
					BashHelper.ExecBashCommand("echo '-' >> /home/pi/.config/pianobar/ctl");
					message = "Thumb Down";
					break;
				case PandoraCommandEnum.VolumeUp:
					BashHelper.ExecBashCommand("echo '))' >> /home/pi/.config/pianobar/ctl");
					message = "Volume Up";
					break;	
				case PandoraCommandEnum.VolumeDown:
					BashHelper.ExecBashCommand("echo '((' >> /home/pi/.config/pianobar/ctl");
					message = "Volume down";
					break;
				case PandoraCommandEnum.Tired:
					BashHelper.ExecBashCommand("echo 't' >> /home/pi/.config/pianobar/ctl");
					message = "Tired of this song";
					break;
				default:
					throw new Exception($"Command {command} is not defined");
			}

			return new Result()
			{
				ErrorCode = 0,
				Message = message,
				Ok = true
			};
		}
	}
}
