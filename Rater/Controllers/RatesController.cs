using Microsoft.AspNetCore.Mvc;
using Rater.Models;

namespace Rater.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RatesController : ControllerBase
    {
        private readonly ILogger<RatesController> _logger;

        public RatesController(ILogger<RatesController> logger)
        {
            _logger = logger;
        }

        [HttpPost("/cron")]
        public async Task<ActionResult> CreateRatesScheduled(CancellationToken cancellationToken)
        {
            try
            {
                var from = DateTime.Today.AddDays(1);
                var to = from.AddMonths(1);
                await GenerateRates(from, to);
                return Ok();
            }
            catch
            {
                return Conflict();
            }
            //todo: To test: In order to tell Dapr that the event was not processed correctly in your application and schedule it for redelivery, return any response other than 200 OK.For example, a 500 Error.
        }

        [HttpPost]
        public async Task<ActionResult> CreateRate([FromBody]RateCreationRequest request)
        {
            await GenerateRates(request.HotelCode, request.Date, request.Date);
            //todo: To test: In order to tell Dapr that the event was not processed correctly in your application and schedule it for redelivery, return any response other than 200 OK.For example, a 500 Error.
            //var rnd = Random.Shared.Next(0, 1);
            //return Convert.ToBoolean(rnd) ? Ok() : Conflict();
            throw new NotImplementedException();

        }

        private Task GenerateRates(string hotelCode, DateTime from, DateTime to)
        {
            for (var current = from; current <= to; current.AddDays(1))
            { 

            }
            var range = to..from;
            foreach (to..)
        }

        private void GenerateRandomError()
        {
            var rnd = Random.Shared.Next(0, 1);
            if (rnd.Equals(0))
            {
                throw new Exception();
            }
        }

        private async Task<Rate> CalculateRate(string hotelCode, DateTime date)
        {
            var hotelInfo = await GetHotelInformation(hotelCode);
            var capacity = await GetHotelCapacityForecast(hotelCode);

            var baseRate = hotelInfo.BaseRate;
            var desviation = capacity.OccupancyPercentage - 0.5;
            var desviationMultiplier = desviation / 2;
            var confidenceRate = capacity.ConfidenceRate;
            var confidenceMultiplier = Math.Sqrt(confidenceRate);
            var rate = baseRate + (baseRate * desviationMultiplier * confidenceMultiplier);
            return new Rate(date, rate);
        }

        private Task<HotelInfo> GetHotelInformation(string hotelCode)
        { 
            throw new NotImplementedException();
        }

        private Task<CapacityForecast> GetHotelCapacityForecast(string hotelCode)
        {
            throw new NotImplementedException();
        }
    }
}