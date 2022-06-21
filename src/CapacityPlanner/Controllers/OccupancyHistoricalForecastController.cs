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

        [Topic("servicebus-pubsub", "capacityforecast", "event.source == \"historical\"", 1)]
        [HttpPost]
        public async Task Create([FromBody] OccupancyHistoricalForecast historicalForecast, CancellationToken cancellationToken)
        {
            var capacityForecastValue = GetCapacityForecastValue(historicalForecast);
            var capacityForecast = new CapacityForecast(historicalForecast.HotelCode, historicalForecast.Date, capacityForecastValue, CONFIDENCE_RATE);
            await _capacityForecastService.SaveCapacityForecast(capacityForecast, cancellationToken);

            _logger.LogTrace($"Hitorical capacity forecast created {capacityForecast.HotelCode} - {capacityForecast.OccupancyPercentage}%");
        }

        private double GetCapacityForecastValue(OccupancyHistoricalForecast forecast) =>
            forecast.HistoricalLevel / 10.0;
    }
}