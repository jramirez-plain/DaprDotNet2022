using Dapr.Client;
using HotelCatalog.Models;

namespace HotelCatalog.Services
{
    public class HotelCatalogService
    {
        private readonly DaprClient _daprClient;

        private string STORE_NAME = "cosmosdb-state"; //"redis-state"; //"blobstorage-state";

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

        public async Task<IEnumerable<Hotel>> GetHotels(string countryCode, CancellationToken cancellationToken)
        {
            var jsonQuery = @$"
            {{
                ""filter"": {{
                    ""EQ"": {{ ""countryCode"": ""{countryCode}""}}
                }}
            }}";

            var metaproperties = new Dictionary<string, string>
            {
                ["queryIndexName"] = "countryIndex"
            };

            var queryResponse = await _daprClient.QueryStateAsync<Hotel>(STORE_NAME, jsonQuery, metaproperties, cancellationToken: cancellationToken);
            return queryResponse.Results.Select(x => x.Data);
        }       
    }
}
