using Microsoft.AspNet.Identity;
using SmartHouseWeb.Models;
using SmartHouseWeb.SignalRHubs;
using SmartHouseWebLib.DomainService.Interface;
using SmartHouseWebLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace SmartHouseWeb.Controllers.Api
{
    [Authorize]
    [RoutePrefix("api/Telemetry")]
    public class TelemetryController : ApiController
    {
        private readonly ITelemetryDataService telemetryDataService;
        private readonly IRoomService roomService;

        public TelemetryController(ITelemetryDataService telemetryDataService, IRoomService roomService)
        {
            this.telemetryDataService = telemetryDataService;
            this.roomService = roomService;
        }

        [HttpPost]
        [Route("SaveTelemetryData")]
        public async Task<IHttpActionResult> SaveTelemetryData(TelemetryDataDto telemetryDataDto)
        {
            var userId = User.Identity.GetUserId();
            var rooms = await roomService.GetAllAsync();

            var telemetryData = new TelemetryData()
            {
                CreatedUtc = telemetryDataDto.CreatedUtc,
                GasValue = telemetryDataDto.GasValue,
                Humidity = telemetryDataDto.Humidity,
                RoomId = rooms.FirstOrDefault().Id,
                Temperature = telemetryDataDto.Temperature,
                HeatIndex = telemetryDataDto.HeatIndex,
            };

            await telemetryDataService.Insert(telemetryData);

            var hub = new TelemetryHub();
            hub.NotifyClient(telemetryDataDto, userId);

            return Ok(telemetryData.Id);
        }

        [HttpGet]
        [Route("GetLastTelemetry")]
        public async Task<TelemetryDataDto> GetLastTelemetry()
        {
            var userId = User.Identity.GetUserId();
            var telemetry = await telemetryDataService.GetLastAsync(userId);

            return new TelemetryDataDto()
            {
                CreatedUtc = telemetry.CreatedUtc,
                GasValue = telemetry.GasValue,
                HeatIndex = telemetry.HeatIndex,
                Humidity = telemetry.Humidity,
                RoomId = telemetry.RoomId,
                Temperature = telemetry.Temperature
            };
        }

    }
}
