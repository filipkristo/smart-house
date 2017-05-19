using Microsoft.AspNet.Identity;
using SmartHouseWeb.Models;
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

            var telemetryData = new TelemetryData()
            {
                CreatedUtc = telemetryDataDto.CreatedUtc,
                GasValue = telemetryDataDto.GasValue,
                Humidity = telemetryDataDto.Humidity,
                RoomId = telemetryDataDto.RoomId,
                Temperature = telemetryDataDto.Temperature,
                HeatIndex = telemetryDataDto.HeatIndex
                //UserId = userId
            };

            await telemetryDataService.Insert(telemetryData);
            return Ok(telemetryData.Id);
        }
    }
}
