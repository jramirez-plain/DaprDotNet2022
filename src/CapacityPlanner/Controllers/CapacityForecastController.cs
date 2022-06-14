using CapacityPlanner.Models;
using Microsoft.AspNetCore.Mvc;

namespace CapacityPlanner.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CapacityForecastController : ControllerBase
    {
        private readonly ILogger<CapacityForecastController> _logger;

        public CapacityForecastController(ILogger<CapacityForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "CreateOccupancyHistoricalForecast")]
        public async Task<ActionResult<CapacityForecast>> Get(DateTime datetime, CancellationToken cancellationToken)
        {

            var capacityForecast = await Retrieve(datetime, cancellationToken);
            if (capacityForecast is null)
            {
                capacityForecast = new(OccupancyPercentage: 0.5, ConfidenceRate: 0.0);
            }
            return Ok(capacityForecast);
        }


        private Task<CapacityForecast> Retrieve(DateTime dateTime, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

    }
}