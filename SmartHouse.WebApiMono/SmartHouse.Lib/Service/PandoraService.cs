using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SmartHouse.Lib
{
    public class PandoraService : BasePlayerService, IPlayerService
    {
        public PandoraService()
        {
        }

        public Result Play()
        {
            return CommandExecuter(PlayerCommandEnum.Play);
        }

        public Result Pause()
        {
            return CommandExecuter(PlayerCommandEnum.Pause);
        }

        public async Task<Result> Start()
        {
            var result = CommandExecuter(PlayerCommandEnum.Start);
            await Task.Delay(TimeSpan.FromSeconds(6));

            var info = GetCurrentSongInfo();
            var stations = await GetStationList().ConfigureAwait(false);

            var station = stations.FirstOrDefault(x => x.Value.Contains(info.Radio));
            var key = station?.Key ?? "1";

            BashHelper.ExecBashCommandNoWait($"echo '{key}' >> /home/pi/.config/pianobar/ctl");

            return result;
        }

        public Result Stop()
        {
            return CommandExecuter(PlayerCommandEnum.Stop);
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

        public Result Prev()
        {
            return new Result
            {
                ErrorCode = 1,
                Message = "Not supported",
                Ok = true
            };
        }

        public Result Next()
        {
            return CommandExecuter(PlayerCommandEnum.Next);
        }

        public Result ThumbUp()
        {
            return CommandExecuter(PlayerCommandEnum.ThumbUp);
        }

        public Result ThumbDown()
        {
            return CommandExecuter(PlayerCommandEnum.ThumbDown);
        }

        public Result TiredOfThisSong()
        {
            return CommandExecuter(PlayerCommandEnum.Tired);
        }

        public Result VolumeUp()
        {
            return CommandExecuter(PlayerCommandEnum.VolumeUp);
        }

        public Result VolumeDown()
        {
            return CommandExecuter(PlayerCommandEnum.VolumeDown);
        }

        public override Result ChangeStation(string stationId)
        {
            BashHelper.ExecBashCommand($"echo 's{stationId}' >> /home/pi/.config/pianobar/ctl");

            return new Result()
            {
                ErrorCode = 0,
                Message = $"Changed station {stationId}",
                Ok = true
            };
        }
        
        public override PandoraResult GetCurrentSongInfo()
        {
            var homeDir = System.Environment.GetEnvironmentVariable("HOME");
            var path = Path.Combine(homeDir ?? throw new NullReferenceException("Can't get home directory"), @".config/pianobar/nowplaying");

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

        public Task<PandoraResult> GetNowPlaying()
        {
            return Task.Run(() =>
            {
                var result = GetCurrentSongInfo();

                return new PandoraResult()
                {
                    Album = result.Album,
                    AlbumUri = result.AlbumUri,
                    Artist = result.Artist,
                    DurationSeconds = result.DurationSeconds,
                    Loved = result.Loved,
                    Song = result.Song,
                    Radio = result.Radio
                };
            });
        }

        public async Task<Result> LoveSong()
        {
            var lastFMService = new LastFMService();

            var songInfo = await GetNowPlaying();

            if (songInfo.Loved)
                return new Result()
                {
                    Ok = true,
                    ErrorCode = 0,
                    Message = "You already liked this song"
                };

            var status = await lastFMService.LoveSong(songInfo.Artist, songInfo.Song);
            ThumbUp();

            return new Result
            {
                Ok = true,
                ErrorCode = 0,
                Message = $"You liked {songInfo.Song} song. Status: {status}"
            };
        }

        public override Task<IEnumerable<KeyValue>> GetStationList()
        {
            var homeDir = System.Environment.GetEnvironmentVariable("HOME");
            var path = Path.Combine(homeDir, @".config/pianobar/stationlist");

            return Task.Run(() =>
            {
                return File.ReadLines(path)
                        .Select(x => new KeyValue
                        {
                            Key = ParseKey(x),
                            Value = ParseName(x)
                        });

                string ParseKey(string name)
                {
                    return name.Replace(")", "").Substring(0, name.Replace(")", "").IndexOf(' '));
                }

                string ParseName(string name)
                {
                    var nameWithDigit = name.Replace(")", "");
                    return Regex.Replace(nameWithDigit, "[0-9]", "").Trim();
                }
            });
        }

        public Task<bool> IsPlaying()
        {
            return Task.FromResult(GetCurrentSongInfo().IsPlaying);
        }

        private Result CommandExecuter(PlayerCommandEnum command)
        {
            var message = string.Empty;
            switch (command)
            {
                case PlayerCommandEnum.Play:
                    BashHelper.ExecBashCommand("echo 'p' >> /home/pi/.config/pianobar/ctl");
                    message = "Play/Pause";
                    break;
                case PlayerCommandEnum.Pause:
                    BashHelper.ExecBashCommand("echo 'p' >> /home/pi/.config/pianobar/ctl");
                    message = "Play/Pause";
                    break;
                case PlayerCommandEnum.Start:
                    BashHelper.ExecBashScriptNoWait("./pandora.sh start");
                    message = "Start";
                    break;
                case PlayerCommandEnum.Stop:
                    BashHelper.ExecBashScriptNoWait("./pandora.sh stop");
                    message = "Stop";
                    break;
                case PlayerCommandEnum.Next:
                    BashHelper.ExecBashCommand("echo 'n' >> /home/pi/.config/pianobar/ctl");
                    message = "Next";
                    break;
                case PlayerCommandEnum.ThumbUp:
                    BashHelper.ExecBashCommand("echo '+' >> /home/pi/.config/pianobar/ctl");
                    message = "Thumb Up";
                    break;
                case PlayerCommandEnum.ThumbDown:
                    BashHelper.ExecBashCommand("echo '-' >> /home/pi/.config/pianobar/ctl");
                    message = "Thumb Down";
                    break;
                case PlayerCommandEnum.VolumeUp:
                    BashHelper.ExecBashCommand("echo '))' >> /home/pi/.config/pianobar/ctl");
                    message = "Volume Up";
                    break;	
                case PlayerCommandEnum.VolumeDown:
                    BashHelper.ExecBashCommand("echo '((' >> /home/pi/.config/pianobar/ctl");
                    message = "Volume down";
                    break;
                case PlayerCommandEnum.Tired:
                    BashHelper.ExecBashCommand("echo 't' >> /home/pi/.config/pianobar/ctl");
                    message = "Tired of this song";
                    break;
                case PlayerCommandEnum.ChangeStation:
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

        public string GetAmplifierInput() => "HDMI1";
    }
}
