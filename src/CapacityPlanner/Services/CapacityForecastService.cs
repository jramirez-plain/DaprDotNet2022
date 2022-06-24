using CapacityPlanner.Models;

namespace CapacityPlanner.Services
{
    public class CapacityForecastService
    {
        public Task SaveCapacityForecast(CapacityForecast capacityForecast, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();      
        }

        public Task<CapacityForecast> RetrieveCapacityForecast(string hotelCode, DateTime date, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<int> GetTotalCapacity(string hotelCode, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
            //HotelInfo hotelInformation;
            //return hotelInformation.RoomNumber;
        }

        private record CapacityForecastKey(string hotelCode, DateTime date)
        {
            public string Key => $"hotelCode={hotelCode}:date={date.ToString("s")}";
        }
    }
}
