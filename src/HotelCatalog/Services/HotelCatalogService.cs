using Dapr.Client;
using HotelCatalog.Models;

namespace HotelCatalog.Services
{
    public class HotelCatalogService
    {
        private readonly DaprClient _daprClient;
        private readonly ILogger<HotelCatalogService> _logger;


        private string STORE_NAME = "cosmosdb-state"; //"redis-state"; //"blobstorage-state";
        public HotelCatalogService(DaprClient daprClient, ILogger<HotelCatalogService> logger)
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

        public async Task<IEnumerable<Hotel>> GetHotelsWithQuery(string countryCode, CancellationToken cancellationToken)
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

        public async Task<IEnumerable<Hotel>> GetHotelsWithFilter(string countryCode, CancellationToken cancellationToken)
        {
            var response = await _daprClient.GetStateAsync<IEnumerable<Hotel>>(STORE_NAME, "hotels", cancellationToken: cancellationToken);

            return response.Any() ?
                response.Where(h => h.CountryCode == countryCode) :
                Enumerable.Empty<Hotel>();
        }
    }
}
