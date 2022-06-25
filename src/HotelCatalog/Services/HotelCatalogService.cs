using Dapr.Client;
using HotelCatalog.Models;

namespace HotelCatalog.Services
{
    public class HotelCatalogService
    {
        public async Task<Hotel> GetHotel(string code, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task SaveOrUpdateHotel(Hotel hotel, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
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
