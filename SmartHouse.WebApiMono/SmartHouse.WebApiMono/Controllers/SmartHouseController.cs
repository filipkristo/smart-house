using System;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Libmpc;
using SmartHouse.Lib;

namespace SmartHouse.WebApiMono
{
	[RoutePrefix("api/SmartHouse")]
	public class SmartHouseController : BaseController
	{
		private readonly IYamahaService YamahaService;
		private readonly IPanodraService PandoraService;
		private readonly ISmartHouseService SmartHouseService;
		private readonly IMPDService MpdService;

		public SmartHouseController(ISettingsService service, IYamahaService yamahaService, IPanodraService pandoraService, ISmartHouseService smartHouseService, IMPDService mpdService) 
			: base(service)
		{
			YamahaService = yamahaService;
			PandoraService = pandoraService;
			SmartHouseService = smartHouseService;
			MpdService = mpdService;
		}

		[HttpGet]
		[Route("TurnOn")]
		public async Task<Result> TurnOn()
		{
			var sb = new StringBuilder();
			var powerStatus = await YamahaService.PowerStatus();

			if (powerStatus == PowerStatusEnum.StandBy)
			{
				await YamahaService.TurnOn();
				sb.AppendLine("Yamaha Turn on");
				await Task.Delay(TimeSpan.FromSeconds(8));
			}

			await YamahaService.SetInput("HDMI1");
			sb.AppendLine("Setting HDMI1 input");

			await SmartHouseService.SetMode(ModeEnum.Normal);
			sb.AppendLine("Setting Normal mode");

			var state = await SmartHouseService.GetCurrentState();

			if (state == SmartHouseState.Music && MpdService.GetStatus().State != Libmpc.MpdState.Play)
			{
				PandoraService.StopTcp().Wait(1000);
				MpdService.Play();
				sb.AppendLine("Playing MPD");
				await SmartHouseService.SaveState(SmartHouseState.Music);
			}

			else if (!PandoraService.IsPlaying())
			{
				PandoraService.StartTcp().Wait(1000);
				PandoraService.Play();
				sb.AppendLine("Playing pandora radio");	
				await SmartHouseService.SaveState(SmartHouseState.Pandora);
			}

			return new Result()
			{
				ErrorCode = 0,
				Message = sb.ToString(),
				Ok = true
			};
		}

		[HttpGet]
		[Route("TurnOff")]
		public async Task<Result> TurnOff()
		{
			var sb = new StringBuilder();
			var powerStatus = await YamahaService.PowerStatus();

			if (PandoraService.IsPlaying())
			{
				PandoraService.Pause();
				sb.AppendLine("Pausing pandora radio");	
			}

			if (MpdService.GetStatus().State == Libmpc.MpdState.Play)
			{
				MpdService.Stop();
				sb.AppendLine("Stopping MPD");
			}

			if (powerStatus == PowerStatusEnum.On)
			{
				await Task.Delay(TimeSpan.FromSeconds(1));
				await YamahaService.TurnOff();
				sb.AppendLine("Yamaha Turn Off");
			}

			return new Result()
			{
				ErrorCode = 0,
				Message = sb.ToString(),
				Ok = true
			};
		}

		[HttpGet]
		[Route("VolumeUp")]
		public async Task<Result> VolumeUp()
		{
			var sb = new StringBuilder();
			var powerStatus = await YamahaService.PowerStatus();

			if (powerStatus == PowerStatusEnum.On)
			{
				await YamahaService.VolumeUp();
				sb.AppendLine("Yamaha Volume Up");
			}
			else
			{
				sb.AppendLine("Yamaha is turned off");
			}

			return new Result()
			{
				ErrorCode = 0,
				Message = sb.ToString(),
				Ok = true
			};
		}

		[HttpGet]
		[Route("VolumeDown")]
		public async Task<Result> VolumeDown()
		{
			var sb = new StringBuilder();
			var powerStatus = await YamahaService.PowerStatus();

			if (powerStatus == PowerStatusEnum.On)
			{
				await YamahaService.VolumeDown();
				sb.AppendLine("Yamaha Volume Down");
			}
			else
			{
				sb.AppendLine("Yamaha is turned off");	
			}

			return new Result()
			{
				ErrorCode = 0,
				Message = sb.ToString(),
				Ok = true
			};
		}

		[HttpGet]
		[Route("Next")]
		public async Task<Result> Next()
		{
			var sb = new StringBuilder();
			var powerStatus = await YamahaService.PowerStatus();

			if (powerStatus == PowerStatusEnum.On)
			{
				var mpdState = MpdService.GetStatus().State;

				if (mpdState == MpdState.Play || mpdState == MpdState.Pause)
				{
					MpdService.Next();
					sb.AppendLine("MPD Next song");
				}
				else
				{
					PandoraService.Next();
					sb.AppendLine("Pandora next song");
				}
			}
			else
			{
				sb.AppendLine("Yamaha is turned off. Operation canceled");
			}

			return new Result()
			{
				ErrorCode = 0,
				Message = sb.ToString(),
				Ok = true
			};
		}

		[HttpGet]
		[Route("Play")]
		public async Task<Result> Play()
		{
			var sb = new StringBuilder();
			var powerStatus = await YamahaService.PowerStatus();

			if (powerStatus == PowerStatusEnum.On)
			{
				var state = await SmartHouseService.GetCurrentState();

				if (state == SmartHouseState.Pandora)
				{
					PandoraService.Play();
					sb.AppendLine("Starting to play/pause Pandora");
				}
				else if(state == SmartHouseState.Music)
				{
					if (MpdService.GetStatus().State == MpdState.Play)
						MpdService.Pause();
					else if (MpdService.GetStatus().State == MpdState.Pause)
						MpdService.Play();
				}
			}
			else
			{
				sb.AppendLine("Yamaha is turned off. Operation canceled");
			}

			return new Result()
			{
				ErrorCode = 0,
				Message = sb.ToString(),
				Ok = true
			};
		}


		[HttpGet]
		[Route("Prev")]
		public async Task<Result> Prev()
		{
			var sb = new StringBuilder();
			var powerStatus = await YamahaService.PowerStatus();

			if (powerStatus == PowerStatusEnum.On)
			{
				var mpdState = MpdService.GetStatus().State;

				if (mpdState == MpdState.Play || mpdState == MpdState.Pause)
				{
					MpdService.Previous();
					sb.AppendLine("MPD Previous song");
				}
				else
				{
					PandoraService.Next();
					sb.AppendLine("Pandora next song. Pandora can't go previous");
				}
			}
			else
			{
				sb.AppendLine("Yamaha is turned off. Operation canceled");
			}

			return new Result()
			{
				ErrorCode = 0,
				Message = sb.ToString(),
				Ok = true
			};
		}

		[HttpGet]
		[Route("SetMode")]
		public async Task<Result> SetMode(string mode)
		{
			var modeEnum = (ModeEnum)Enum.Parse(typeof(ModeEnum), mode);

			var result = await SmartHouseService.SetMode(modeEnum);
			return result;
		}

		[HttpGet]
		[Route("Xbox")]
		public async Task<Result> Xbox()
		{
			var sb = new StringBuilder();
			var powerStatus = await YamahaService.PowerStatus();

			if (powerStatus == PowerStatusEnum.StandBy)
			{
				await YamahaService.TurnOn();
				sb.AppendLine("Yamaha Turn on");
				await Task.Delay(TimeSpan.FromSeconds(8));
			}

			if (MpdService.GetStatus().State == Libmpc.MpdState.Play)
			{
				MpdService.Stop();
				sb.AppendLine("Stopping MPD");
			}

			if (PandoraService.IsPlaying())
			{
				PandoraService.Pause();
				sb.AppendLine("Pausing pandora radio");
			}				

			await YamahaService.SetInput("HDMI2");
			sb.AppendLine("Set HDMI2 input");

			await SmartHouseService.SaveState(SmartHouseState.XBox);

			return new Result()
			{
				ErrorCode = 0,
				Message = sb.ToString(),
				Ok = true
			};
		}

		[HttpGet]
		[Route("Pandora")]
		public async Task<Result> Pandora()
		{
			var sb = new StringBuilder();
			var powerStatus = await YamahaService.PowerStatus();

			if (powerStatus == PowerStatusEnum.StandBy)
			{
				await YamahaService.TurnOn();
				sb.AppendLine("Yamaha Turn on");
				await Task.Delay(TimeSpan.FromSeconds(8));
			}

			if (MpdService.GetStatus().State == Libmpc.MpdState.Play)
			{
				MpdService.Stop();
				sb.AppendLine("Stopping MPD");
			}


			if (!PandoraService.IsPlaying())
			{
				PandoraService.StartTcp().Wait(1000);

				PandoraService.Play();
				sb.AppendLine("Playing pandora radio");
			}

			await YamahaService.SetInput("HDMI1");
			sb.AppendLine("Set HDMI1 input");

			await SmartHouseService.SaveState(SmartHouseState.Pandora);

			return new Result()
			{
				ErrorCode = 0,
				Message = sb.ToString(),
				Ok = true
			};
		}

		[HttpGet]
		[Route("Music")]
		public async Task<Result> Music()
		{
			var sb = new StringBuilder();
			var powerStatus = await YamahaService.PowerStatus();

			if (powerStatus == PowerStatusEnum.StandBy)
			{
				await YamahaService.TurnOn();
				sb.AppendLine("Yamaha Turn on");
				await Task.Delay(TimeSpan.FromSeconds(8));
			}

			await PandoraService.StopTcp();
			sb.AppendLine("Stopping pandora radio");

			await YamahaService.SetInput("HDMI1");
			sb.AppendLine("Set HDMI1 input");

			MpdService.Play();

			await SmartHouseService.SaveState(SmartHouseState.Music);

			return new Result()
			{
				ErrorCode = 0,
				Message = sb.ToString(),
				Ok = true
			};
		}

		[HttpGet]
		[Route("TV")]
		public async Task<Result> TV()
		{
			var sb = new StringBuilder();
			var powerStatus = await YamahaService.PowerStatus();

			if (powerStatus == PowerStatusEnum.StandBy)
			{
				await YamahaService.TurnOn();
				sb.AppendLine("Yamaha Turn on");
				await Task.Delay(TimeSpan.FromSeconds(8));
			}

			if (MpdService.GetStatus().State == Libmpc.MpdState.Play)
			{
				MpdService.Stop();
				sb.AppendLine("Stopping MPD");
			}

			if (PandoraService.IsPlaying())
			{
				PandoraService.Pause();	
				sb.AppendLine("Pause pandora radio");
			}

			await YamahaService.SetInput("AUDIO1");
			sb.AppendLine("Setting AUDIO1 input");

			await SmartHouseService.SaveState(SmartHouseState.TV);

			return new Result()
			{
				ErrorCode = 0,
				Message = sb.ToString(),
				Ok = true
			};
		}

		[HttpGet]
		[Route("GetCurrentState")]
		public async Task<string> GetCurrentState()
		{
			var state = await SmartHouseService.GetCurrentState();
			return state.ToString();
		}

		[HttpGet]
		[Route("RestartOpenVPN")]
		public async Task<Result> RestartOpenVPN()
		{
			var result = await SmartHouseService.RestartOpenVPNServiceTcp();
			return result;
		}

		[HttpGet]
		[Route("PlayAlarm")]
		public async Task<Result> PlayAlarm()
		{
			var result = await SmartHouseService.PlayAlarmTcp();
			return result;
		}

		[HttpGet]
		[Route("TurnOfTimer")]
		public async Task<Result> TurnOfTimer(int minutes)
		{
			Timer.TimeoutMinutes = minutes;
			var result = await SmartHouseService.TimerTcp();
			return result;
		}
	}
}
