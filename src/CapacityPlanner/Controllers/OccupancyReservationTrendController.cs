using CapacityPlanner.Models;
using CapacityPlanner.Services;
using Dapr;
using Microsoft.AspNetCore.Mvc;

namespace CapacityPlanner.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OccupancyReservationTrendController : ControllerBase
    {
        private readonly ILogger<OccupancyReservationTrendController> _logger;
        private readonly CapacityForecastService _capacityForecastService;

        private double CONFIDENCE_RATE = 0.75;
        public OccupancyReservationTrendController(CapacityForecastService capacityForecastService, ILogger<OccupancyReservationTrendController> logger)
        {
            _capacityForecastService = capacityForecastService;
            _logger = logger;
        }

        [Topic("servicebus-pubsub", "capacity-forecast", "event.data.type = 'reservations'", 2)]
        [HttpPost]
        public async Task Create(OccupancyReservationTrend reservationTrend, CancellationToken cancellationToken)
        {
            var capacityForecastValue = await GetCapacityForecastValue(reservationTrend, cancellationToken);
            var capacityForecast = new CapacityForecast(reservationTrend.HotelCode, capacityForecastValue, CONFIDENCE_RATE);
            await _capacityForecastService.SaveCapacityForecast(capacityForecast, cancellationToken);
        }

        private async Task<double> GetCapacityForecastValue(OccupancyReservationTrend forecast, CancellationToken cancellationToken)
        {
            var totalCapacity = await _capacityForecastService.GetTotalCapacity(forecast.HotelCode, cancellationToken);
            return forecast.EstimatedReservations / totalCapacity;
        }
    }
}