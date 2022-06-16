using CapacityPlanner.Models;

namespace CapacityPlanner.Services
{
    public interface ICapacityForecastService
    {
        Task<int> GetTotalCapacity(CancellationToken cancellationToken);
        Task<CapacityForecast> RetrieveCapacityForecast(DateTime dateTime, CancellationToken cancellationToken);
        Task SaveCapacityForecast(CapacityForecast capacityForecast, CancellationToken cancellationToken);
    }
}