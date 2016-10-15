using System;
using System.Collections.Generic;
using System.Web.Http;
using SmartHouse.Lib;

namespace SmartHouse.WebApiMono
{
	[RoutePrefix("api/Pandora")]
	public class PandoraController : BaseController
	{
		private PandoraCommand command;

		public PandoraController(ISettingsService service) : base(service)
		{
			command = new PandoraCommand();
		}

		[HttpGet]
		[Route("Play")]
		public Result Play()
		{
			return command.Play();
		}

		[HttpGet]
		[Route("Stop")]
		public Result Stop()
		{
			return command.Stop();
		}

		[HttpGet]
		[Route("Next")]
		public Result Next()
		{
			return command.Next();
		}

		[HttpGet]
		[Route("ThumbUp")]
		public Result ThumbUp()
		{
			return command.ThumbUp();
		}

		[HttpGet]
		[Route("ThumbDown")]
		public Result ThumbDown()
		{
			return command.ThumbDown();
		}

		[HttpGet]
		[Route("VolumeUp")]
		public Result VolumeUp()
		{
			return command.VolumeUp();
		}

		[HttpGet]
		[Route("VolumeDown")]
		public Result VolumeDown()
		{
			return command.VolumeDown();
		}

		[HttpGet]
		[Route("ChangeStation")]
		public Result ChangeStation(string stationId)
		{
			return command.ChangeStation(stationId);
		}

		[HttpGet]
		[Route("CurrentSongInfo")]
		public PandoraResult CurrentSongInfo()
		{
			return command.GetCurrentSongInfo();
		}

		[HttpGet]
		[Route("StationList")]
		public IEnumerable<KeyValue> StationList()
		{
			return command.GetStationList();
		}

	}
}
