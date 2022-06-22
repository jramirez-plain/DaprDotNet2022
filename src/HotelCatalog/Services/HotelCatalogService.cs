using Dapr.Client;
using HotelCatalog.Models;

namespace HotelCatalog.Services
{
    public class HotelCatalogService
    {
        private readonly DaprClient _daprClient;
        private readonly Logger<HotelCatalogService> _logger;


        private string STORE_NAME = "cosmosdb-state"; //"redis-state"; //"blobstorage-state";
        public HotelCatalogService(DaprClient daprClient, Logger<HotelCatalogService> logger)
        {
            _daprClient = daprClient;
            _logger = logger;
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
            try
            {
                return await GetHotelsWithQuery(countryCode, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error get catalog from state with query: {ex.Message}, retry without it....");

                return await GetHotelsWithFilter(countryCode, cancellationToken); throw;
            }
        }


        private async Task<IEnumerable<Hotel>> GetHotelsWithQuery(string countryCode, CancellationToken cancellationToken)
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

        private async Task<IEnumerable<Hotel>> GetHotelsWithFilter(string countryCode, CancellationToken cancellationToken)
        {
            var response = await _daprClient.GetStateAsync<IEnumerable<Hotel>>(STORE_NAME, "hotels", cancellationToken: cancellationToken);

            return response.Where(h => h.CountryCode == countryCode);
        }
    }
}
