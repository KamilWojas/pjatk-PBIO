using Ex5.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ex5.Controllers

{
    [Route("api/trips")]
    [ApiController]
    public class TripsController : ControllerBase
    {
        private readonly ITripsService _tripsService;
        public TripsController(ITripsService tripsService)
        {
            _tripsService = tripsService;
        }


        [HttpGet]
        public async Task<IActionResult> GetTrips()
        {
            var trips = await _tripsService.GetTripsWithAdditionalData();
            return Ok(trips);
        }
    }
}
