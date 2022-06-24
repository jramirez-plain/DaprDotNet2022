using Microsoft.AspNetCore.Mvc;
using Rater.Models;
using Rater.Services;

namespace Rater.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RaterController : ControllerBase
    {
        private readonly RaterService _raterService;
        private readonly ILogger<RaterController> _logger;

        public RaterController(RaterService raterService, ILogger<RaterController> logger)
        {
            _raterService = raterService;
            _logger = logger;
        }

        public async Task<ActionResult> CreateRatesScheduled(CancellationToken cancellationToken)
        {
            try
            {
                //GenerateRandomError();
                var from = DateTime.Today.AddDays(1);
                var to = from.AddMonths(1);
                var countryCode = "ESP";
                var hotels = await _raterService.GetHotels(countryCode, cancellationToken);
                foreach (var hotel in hotels)
                {
                    await _raterService.GenerateRates(hotel, from, to, cancellationToken);
                }
                return Ok();
            }
            catch
            {
                return Conflict();
            }
            //todo: To test: In order to tell Dapr that the event was not processed correctly in your application and schedule it for redelivery, return any response other than 200 OK.For example, a 500 Error.
        }

        [HttpPost]
        public async Task<ActionResult> CreateRate([FromBody] RateCreationRequest request, CancellationToken cancellationToken)
        {
            var hotel = await _raterService.GetHotel(request.HotelCode, cancellationToken);
            await _raterService.GenerateRates(hotel, request.Date, request.Date, cancellationToken);
            return Ok();

        }

        private void GenerateRandomError()
        {
            var rnd = Random.Shared.Next(0, 1);
            if (rnd.Equals(0))
            {
                throw new Exception();
            }
        }
    }
}