using CapacityPlanner.Models;
using CapacityPlanner.Services;
using Dapr;
using Microsoft.AspNetCore.Mvc;

namespace CapacityPlanner.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CapacityForecastController : ControllerBase
    {
        private readonly ILogger<CapacityForecastController> _logger;
        private readonly CapacityForecastService _capacityForecastService;

        public CapacityForecastController(CapacityForecastService capacityForecastService, ILogger<CapacityForecastController> logger)
        {
            _capacityForecastService = capacityForecastService;
            _logger = logger;
        }

        [HttpGet("~/hotels/{hotelCode}/capacityForecasts/{date}")]
        public async Task<ActionResult<CapacityForecast>> Get(string hotelCode, DateTime date, CancellationToken cancellationToken)
        {
            var capacityForecast = await _capacityForecastService.RetrieveCapacityForecast(hotelCode, date, cancellationToken);
            if (capacityForecast is null)
            {
                capacityForecast = CapacityForecast.Default(hotelCode, date);
            }
            return Ok(capacityForecast);
        }

        [Topic("servicebus-pubsub", "capacityforecast")]
        [HttpPost]
        public async Task<ActionResult> Create(CapacityForecast capacityForecast, CancellationToken cancellationToken)
        {
            await _capacityForecastService.SaveCapacityForecast(capacityForecast, cancellationToken);
            return Ok(capacityForecast);
        }

    }
}