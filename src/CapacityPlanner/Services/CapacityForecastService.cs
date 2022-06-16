using CapacityPlanner.Models;
using Dapr.Client;

namespace CapacityPlanner.Services
{
    public class CapacityForecastService
    {
        private readonly DaprClient _daprClient;
        private const string CATALOG = "catalog";
        private const string CATALOG_METHOD = "catalog";
        public CapacityForecastService(DaprClient daprClient)
        {
            _daprClient = daprClient;
        }

        public Task SaveCapacityForecast(CapacityForecast capacityForecast, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<CapacityForecast> RetrieveCapacityForecast(DateTime dateTime, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<int> GetTotalCapacity(CancellationToken cancellationToken)
        {
            var hotelCode = "1"; //todo;
            var hotelInformation = await _daprClient.InvokeMethodAsync<HotelInfo>(
                HttpMethod.Get,
                CATALOG,
                $"{CATALOG_METHOD}/{hotelCode}",
                cancellationToken);
            return hotelInformation.RoomNumber;
        }
    }
}
