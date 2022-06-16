using Microsoft.AspNetCore.Mvc;
using Rater.Models;
using System.Text;

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

        [HttpPost("/rater-cron")]
        public async Task<ActionResult> CreateRatesScheduled(CancellationToken cancellationToken)
        {
            try
            {
                GenerateRandomError();
                var from = DateTime.Today.AddDays(1);
                var to = from.AddMonths(1);
                var hotels = await GetHotels();
                foreach (var hotel in hotels)
                {
                    await GenerateRates(hotel, from, to);
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
        public async Task<ActionResult> CreateRate([FromBody] RateCreationRequest request)
        {
            var hotel = await GetHotel(request.HotelCode);
            await GenerateRates(hotel, request.Date, request.Date);

            throw new NotImplementedException();

        }

        private void GenerateRandomError()
        {
            var rnd = Random.Shared.Next(0, 1);
            if (rnd.Equals(0))
            {
                throw new Exception();
            }
        }

        private async Task GenerateRates(HotelInfo hotel, DateTime from, DateTime to)
        {
            var rates = await CalculateRates(hotel, from, to);
            await SendRatesMail(hotel, rates);

        }

        private async Task<IEnumerable<Rate>> CalculateRates(HotelInfo hotel, DateTime from, DateTime to)
        {
            var rates = new List<Rate>();
            for (var current = from; current <= to; current.AddDays(1))
            {
                rates.Add(await CalculateRate(hotel, current));
            }
            return rates;
        }

        private async Task<Rate> CalculateRate(HotelInfo hotel, DateTime date)
        {
            var capacity = await GetHotelCapacityForecast(hotel.Code, date);

            var baseRate = hotel.BaseRate;
            var desviation = capacity.OccupancyPercentage - 0.5;
            var desviationMultiplier = desviation / 2;
            var confidenceRate = capacity.ConfidenceRate;
            var confidenceMultiplier = Math.Sqrt(confidenceRate);
            var rate = baseRate + (baseRate * desviationMultiplier * confidenceMultiplier);
            return new Rate(date, rate);
        }

        private Task<IEnumerable<HotelInfo>> GetHotels()
        {
            throw new NotImplementedException();
        }

        private Task<HotelInfo> GetHotel(string hotelCode)
        {
            throw new NotImplementedException();
        }

        private Task<CapacityForecast> GetHotelCapacityForecast(string hotelCode, DateTime date)
        {
            throw new NotImplementedException();
        }

        private Task SendRatesMail(HotelInfo hotel, IEnumerable<Rate> rates)
        {
            var body = GenerateMailBody(hotel, rates);
            throw new NotImplementedException();
        }

        private string GenerateMailBody(HotelInfo hotel, IEnumerable<Rate> rates)
        {
            var textBuilder = new StringBuilder();
            textBuilder.AppendLine($"Rates for hotel {hotel.Name}");
            foreach (var rate in rates)
            {
                textBuilder.AppendLine($"Date {rate.Date}: {rate.Price}€");
            }
            return textBuilder.ToString();
        }
    }
}