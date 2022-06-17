using Dapr.Client;
using System.Linq;
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

        public Task<Hotel> GetHotel(string code, CancellationToken cancellationToken)
        {
            return _daprClient.GetStateAsync<Hotel>(STORE_NAME, code, cancellationToken: cancellationToken);
        }

        public Task SaveOrUpdateHotel(Hotel hotel, CancellationToken cancellationToken)
        {
             return _daprClient.SaveStateAsync(STORE_NAME, hotel.Code, hotel, cancellationToken: cancellationToken);
        }

        public async Task<IEnumerable<Hotel>> GetHotelsByCountry(string countryCode, CancellationToken cancellationToken)
        {
            var query = @$"
                ""filter"": {{
                    ""EQ"": {{ ""countryCode"": ""{countryCode}""}}
                }}
            }}";
            var queryResponse =  await _daprClient.QueryStateAsync<Hotel>(STORE_NAME, query, cancellationToken: cancellationToken);
            return queryResponse.Results.Select(x => x.Data);
        }
    }
}
