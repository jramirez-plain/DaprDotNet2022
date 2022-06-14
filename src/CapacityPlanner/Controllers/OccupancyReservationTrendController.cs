using CapacityPlanner.Models;
using Microsoft.AspNetCore.Mvc;

namespace CapacityPlanner.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OccupancyReservationTrendController : ControllerBase
    {
        private readonly ILogger<OccupancyReservationTrendController> _logger;

        private double CONFIDENCE_RATE = 0.75;
        public OccupancyReservationTrendController(ILogger<OccupancyReservationTrendController> logger)
        {
            _logger = logger;
        }

        [HttpPost(Name = "CreateOccupancyReservationTrend")]
        public async Task Create(OccupancyReservationTrend reservationTrend, CancellationToken cancellationToken)
        {
            var capacityForecastValue = await GetCapacityForecastValue(reservationTrend, cancellationToken);
            var capacityForecast = new CapacityForecast(capacityForecastValue, CONFIDENCE_RATE);
            await SaveCapacityForecast(capacityForecast, cancellationToken);
        }

        private async Task<double> GetCapacityForecastValue(OccupancyReservationTrend forecast, CancellationToken cancellationToken)
        {
            var totalCapacity = await GetTotalCapacity(cancellationToken);
            return forecast.EstimatedReservations / totalCapacity;
        }

        private Task SaveCapacityForecast(CapacityForecast capacityForecast, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        private Task<int> GetTotalCapacity(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

    }
}