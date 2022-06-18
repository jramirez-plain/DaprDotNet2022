using CapacityPlanner.Models;
using Dapr.Client;

namespace CapacityPlanner.Services
{
    public class CapacityForecastService
    {
        private readonly DaprClient _daprClient;
        private const string CATALOG = "catalog";
        private const string CATALOG_METHOD = "hotels";
        private const string STORE_NAME = "redis-store";
        public CapacityForecastService(DaprClient daprClient)
        {
            _daprClient = daprClient;
        }

        public Task SaveCapacityForecast(CapacityForecast capacityForecast, CancellationToken cancellationToken)
        {
            return _daprClient.SaveStateAsync<CapacityForecast>(STORE_NAME, new CapacityForecastKey(capacityForecast.HotelCode, capacityForecast.Date).Key, capacityForecast, cancellationToken: cancellationToken);
        }

        public Task<CapacityForecast> RetrieveCapacityForecast(string hotelCode, DateTime date, CancellationToken cancellationToken)
        {
            return _daprClient.GetStateAsync<CapacityForecast>(STORE_NAME, new CapacityForecastKey(hotelCode, date).Key, cancellationToken: cancellationToken);
        }

        public async Task<int> GetTotalCapacity(string hotelCode, CancellationToken cancellationToken)
        {
            var hotelInformation = await _daprClient.InvokeMethodAsync<HotelInfo>(
                HttpMethod.Get,
                CATALOG,
                $"{CATALOG_METHOD}/{hotelCode}",
                cancellationToken);
            return hotelInformation.RoomNumber;
        }

        private record CapacityForecastKey(string hotelCode, DateTime date)
        {
            public string Key => $"hotelCode={hotelCode}:date={date}";
        }
    }
}
