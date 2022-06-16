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
        private readonly ICapacityForecastService _capacityForecastService;

        public CapacityForecastController(ICapacityForecastService capacityForecastService, ILogger<CapacityForecastController> logger)
        {
            _capacityForecastService = capacityForecastService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<CapacityForecast>> Get(DateTime datetime, CancellationToken cancellationToken)
        {
            var capacityForecast = await _capacityForecastService.RetrieveCapacityForecast(datetime, cancellationToken);
            if (capacityForecast is null)
            {
                capacityForecast = CapacityForecast.Default();
            }
            return Ok(capacityForecast);
        }

        [Topic("servicebus-pubsub", "capacity-forecast")]
        [HttpPost]
        public async Task<ActionResult> Create(CapacityForecast capacityForecast, CancellationToken cancellationToken)
        {
            await _capacityForecastService.SaveCapacityForecast(capacityForecast, cancellationToken);
            return Ok(capacityForecast);
        }

    }
}