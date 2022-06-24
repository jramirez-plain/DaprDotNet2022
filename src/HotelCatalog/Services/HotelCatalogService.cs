using HotelCatalog.Models;

namespace HotelCatalog.Services
{
    public class HotelCatalogService
    {
        public Task<Hotel> GetHotel(string code, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SaveOrUpdateHotel(Hotel hotel, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Hotel>> GetHotels(string countryCode, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }       
    }
}
