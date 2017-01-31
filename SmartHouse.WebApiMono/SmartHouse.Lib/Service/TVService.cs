using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SmartHouse.Lib
{
    public class TVService : ITVService
    {
        public async Task<Result> Power()
        {
            var text = await SendCommand(IRCommands.KEY_POWER);

            return new Result()
            {
                ErrorCode = 0,
                Message = text,
                Ok = true
            };
        }

        public async Task<Result> Stop()
        {
            var text = await SendCommand(IRCommands.KEY_STOP);

            return new Result()
            {
                ErrorCode = 0,
                Message = text,
                Ok = true
            };
        }

        public async Task<Result> Play()
        {
            var text = await SendCommand(IRCommands.KEY_PLAY);

            return new Result()
            {
                ErrorCode = 0,
                Message = text,
                Ok = true
            };
        }

        public async Task<Result> Pause()
        {
            var text = await SendCommand(IRCommands.KEY_PAUSE);

            return new Result()
            {
                ErrorCode = 0,
                Message = text,
                Ok = true
            };
        }

        public async Task<Result> Record()
        {
            var text = await SendCommand(IRCommands.KEY_RECORD);

            return new Result()
            {
                ErrorCode = 0,
                Message = text,
                Ok = true
            };
        }

        public async Task<Result> Rewind()
        {
            var text = await SendCommand(IRCommands.KEY_REWIND);

            return new Result()
            {
                ErrorCode = 0,
                Message = text,
                Ok = true
            };
        }

        public async Task<Result> Forward()
        {
            var text = await SendCommand(IRCommands.KEY_FORWARD);

            return new Result()
            {
                ErrorCode = 0,
                Message = text,
                Ok = true
            };
        }

        public async Task<Result> Netflix()
        {
            var text = await SendCommand(IRCommands.KEY_VCR);

            return new Result()
            {
                ErrorCode = 0,
                Message = text,
                Ok = true
            };
        }

        public async Task<Result> Source()
        {
            var text = await SendCommand(IRCommands.KEY_CHANNEL);

            return new Result()
            {
                ErrorCode = 0,
                Message = text,
                Ok = true
            };
        }

        public async Task<Result> Info()
        {
            var text = await SendCommand(IRCommands.KEY_INFO);

            return new Result()
            {
                ErrorCode = 0,
                Message = text,
                Ok = true
            };
        }

        public async Task<Result> Option()
        {
            var text = await SendCommand(IRCommands.KEY_OPTION);

            return new Result()
            {
                ErrorCode = 0,
                Message = text,
                Ok = true
            };
        }

        public async Task<Result> Back()
        {
            var text = await SendCommand(IRCommands.KEY_BACK);

            return new Result()
            {
                ErrorCode = 0,
                Message = text,
                Ok = true
            };
        }

        public async Task<Result> Home()
        {
            var text = await SendCommand(IRCommands.KEY_HOME);

            return new Result()
            {
                ErrorCode = 0,
                Message = text,
                Ok = true
            };
        }

        public async Task<Result> Ok()
        {
            var text = await SendCommand(IRCommands.KEY_OK);

            return new Result()
            {
                ErrorCode = 0,
                Message = text,
                Ok = true
            };
        }

        public async Task<Result> Up()
        {
            var text = await SendCommand(IRCommands.KEY_UP);

            return new Result()
            {
                ErrorCode = 0,
                Message = text,
                Ok = true
            };
        }

        public async Task<Result> Left()
        {
            var text = await SendCommand(IRCommands.KEY_LEFT);

            return new Result()
            {
                ErrorCode = 0,
                Message = text,
                Ok = true
            };
        }

        public async Task<Result> Right()
        {
            var text = await SendCommand(IRCommands.KEY_RIGHT);

            return new Result()
            {
                ErrorCode = 0,
                Message = text,
                Ok = true
            };
        }

        public async Task<Result> Down()
        {
            var text = await SendCommand(IRCommands.KEY_DOWN);

            return new Result()
            {
                ErrorCode = 0,
                Message = text,
                Ok = true
            };
        }

        public async Task<Result> KEY_1()
        {
            var text = await SendCommand(IRCommands.KEY_1);

            return new Result()
            {
                ErrorCode = 0,
                Message = text,
                Ok = true
            };
        }

        public async Task<Result> KEY_2()
        {
            var text = await SendCommand(IRCommands.KEY_2);

            return new Result()
            {
                ErrorCode = 0,
                Message = text,
                Ok = true
            };
        }

        public async Task<Result> KEY_3()
        {
            var text = await SendCommand(IRCommands.KEY_3);

            return new Result()
            {
                ErrorCode = 0,
                Message = text,
                Ok = true
            };
        }

        public async Task<Result> KEY_4()
        {
            var text = await SendCommand(IRCommands.KEY_4);

            return new Result()
            {
                ErrorCode = 0,
                Message = text,
                Ok = true
            };
        }

        public async Task<Result> KEY_5()
        {
            var text = await SendCommand(IRCommands.KEY_5);

            return new Result()
            {
                ErrorCode = 0,
                Message = text,
                Ok = true
            };
        }

        public async Task<Result> KEY_6()
        {
            var text = await SendCommand(IRCommands.KEY_6);

            return new Result()
            {
                ErrorCode = 0,
                Message = text,
                Ok = true
            };
        }

        public async Task<Result> KEY_7()
        {
            var text = await SendCommand(IRCommands.KEY_7);

            return new Result()
            {
                ErrorCode = 0,
                Message = text,
                Ok = true
            };
        }

        public async Task<Result> KEY_8()
        {
            var text = await SendCommand(IRCommands.KEY_8);

            return new Result()
            {
                ErrorCode = 0,
                Message = text,
                Ok = true
            };
        }

        public async Task<Result> KEY_9()
        {
            var text = await SendCommand(IRCommands.KEY_9);

            return new Result()
            {
                ErrorCode = 0,
                Message = text,
                Ok = true
            };
        }

        public async Task<Result> KEY_0()
        {
            var text = await SendCommand(IRCommands.KEY_0);

            return new Result()
            {
                ErrorCode = 0,
                Message = text,
                Ok = true
            };
        }

        public async Task<string> SendCommand(IRCommands irCommand)
        {
            return await SendCommandUdp(irCommand);
        }

        private async Task<string> SendCommandUdp(IRCommands irCommand)
        {
            try
            {
                var command = $"irsend SEND_ONCE philips {irCommand}";
                var address = IPAddress.Parse("10.110.166.91");

                var udpClient = new UdpClient();
                udpClient.Connect(address, 5740);

                var bytes = Encoding.ASCII.GetBytes(command);
                await udpClient.SendAsync(bytes, bytes.Length);

                var result = await udpClient.ReceiveAsync();
                var str = Encoding.ASCII.GetString(result.Buffer);

                udpClient.Close();

                return str;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        private async Task<string> SendCommandTcp(IRCommands irCommand)
        {
            try
            {
                var command = $"irsend SEND_ONCE philips {irCommand}";

                using (var tcpClient = new TcpClient("10.110.166.91", 5739))
                {
                    using (var netStream = tcpClient.GetStream())
                    using (var streamReader = new StreamReader(netStream, Encoding.UTF8))
                    using (var streamWritter = new StreamWriter(netStream, Encoding.UTF8) { AutoFlush = true })
                    {
                        tcpClient.SendTimeout = (int)TimeSpan.FromSeconds(10).TotalMilliseconds;
                        tcpClient.ReceiveTimeout = (int)TimeSpan.FromSeconds(10).TotalMilliseconds;

                        Logger.LogDebugMessage($"SendCommandToServer: Writing data to network stream writter");
                        await streamWritter.WriteLineAsync(command);

                        Logger.LogDebugMessage($"SendCommandToServer: Reading data from network stream reader");
                        var response = await streamReader.ReadLineAsync();
                        Logger.LogDebugMessage($"SendCommandToServer: TCP Client length: {response.Length}");

                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
