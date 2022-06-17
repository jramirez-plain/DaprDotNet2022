using CapacityPlanner.Models;
using CapacityPlanner.Services;
using Dapr;
using Microsoft.AspNetCore.Mvc;

namespace CapacityPlanner.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OccupancyHistoricalForecastController : ControllerBase
    {
        private readonly ILogger<OccupancyHistoricalForecastController> _logger;
        private readonly CapacityForecastService _capacityForecastService;

        private double CONFIDENCE_RATE = 0.25;
        public OccupancyHistoricalForecastController(CapacityForecastService capacityForecastService, ILogger<OccupancyHistoricalForecastController> logger)
        {
            _capacityForecastService = capacityForecastService;
            _logger = logger;
        }

        [Topic("servicebus-pubsub", "capacity-forecast", "event.data.type = 'historical'", 1)]
        [HttpPost]
        public async Task Create(OccupancyHistoricalForecast historicalForecast, CancellationToken cancellationToken)
        {
            var capacityForecastValue = GetCapacityForecastValue(historicalForecast);
            var capacityForecast = new CapacityForecast(historicalForecast.HotelCode, capacityForecastValue, CONFIDENCE_RATE);
            await _capacityForecastService.SaveCapacityForecast(capacityForecast, cancellationToken);
        }

        private double GetCapacityForecastValue(OccupancyHistoricalForecast forecast) =>
            forecast.HistoricalLevel / 10.0;
    }
}