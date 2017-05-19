using Microsoft.AspNet.Identity;
using SmartHouseWeb.Models;
using SmartHouseWebLib.DomainService.Interface;
using SmartHouseWebLib.Models;
using System.Threading.Tasks;
using System.Web.Http;

namespace SmartHouseWeb.Controllers.Api
{
    [Authorize]
    [RoutePrefix("api/Location")]
    public class LocationController : ApiController
    {
        private readonly IUserLocationService userLocationService;

        public LocationController(IUserLocationService userLocationService)
        {
            this.userLocationService = userLocationService;
        }

        [HttpPost]
        [Route("SaveUserLocation")]
        public async Task<IHttpActionResult> SaveUserLocation(UserLocationDto userLocationDto)
        {
            var userId = User.Identity.GetUserId();

            var userLocation = new UserLocation()
            {
                Latitude = userLocationDto.Latitude,
                Longitude = userLocationDto.Longitude,
                Name = userLocationDto.Name,
                Status = (LocationStatus)(int)userLocationDto.Status,
                UpdatedUtc = userLocationDto.UpdatedUtc,
                UserId = userId
            };

            await userLocationService.Insert(userLocation);
            return Ok(userLocation.Id);
        }
    }
}
