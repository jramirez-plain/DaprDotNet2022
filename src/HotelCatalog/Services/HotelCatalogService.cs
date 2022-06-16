using Dapr.Client;
using HotelCatalog.Models;

namespace HotelCatalog.Services
{
    public class HotelCatalogService
    {
        private readonly DaprClient _daprClient;
        private string STORE_NAME = "blobstorage-state";
        public HotelCatalogService(DaprClient daprClient)
        {
            _daprClient = daprClient;
        }

        public Task<Hotel> Get(string code, CancellationToken cancellationToken)
        {
            return _daprClient.GetStateAsync<Hotel>(STORE_NAME, code, cancellationToken: cancellationToken);
        }

        public Task Save(Hotel hotel, CancellationToken cancellationToken)
        {
             return _daprClient.SaveStateAsync(STORE_NAME, hotel.Code, hotel, cancellationToken: cancellationToken);
        }
    }
}
