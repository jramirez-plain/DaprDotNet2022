using CapacityPlanner.Models;
using Microsoft.AspNetCore.Mvc;

namespace CapacityPlanner.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OccupancyHistoricalForecastController : ControllerBase
    {
        private readonly ILogger<OccupancyHistoricalForecastController> _logger;

        private double CONFIDENCE_RATE = 0.25;
        public OccupancyHistoricalForecastController(ILogger<OccupancyHistoricalForecastController> logger)
        {
            _logger = logger;
        }

        [HttpPost(Name = "CreateOccupancyHistoricalForecast")]
        public async Task Create(OccupancyHistoricalForecast historicalForecast, CancellationToken cancellationToken)
        {
            var capacityForecast = new CapacityForecast(GetCapacityForecastValue(historicalForecast), CONFIDENCE_RATE);
            await SaveCapacityForecast(capacityForecast, cancellationToken);
        }

        private double GetCapacityForecastValue(OccupancyHistoricalForecast forecast) =>
            forecast.HistoricalLevel / 10.0;


        private Task SaveCapacityForecast(CapacityForecast capacityForecast, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}