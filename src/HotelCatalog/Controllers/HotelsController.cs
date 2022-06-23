using HotelCatalog.Models;
using HotelCatalog.Services;
using Microsoft.AspNetCore.Mvc;

namespace HotelCatalog.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HotelsController : ControllerBase
    {

        private readonly HotelCatalogService _hotelCatalogService;
        private readonly ILogger<HotelsController> _logger;

        public HotelsController(HotelCatalogService hotelCatalogService, ILogger<HotelsController> logger)
        {
            _logger = logger;
            _hotelCatalogService = hotelCatalogService;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Hotel>>> GetList([FromQuery] string countryCode, CancellationToken cancellationToken)
        {
            IEnumerable<Hotel> hotels;
            try
            {
                hotels = await _hotelCatalogService.GetHotelsWithQuery(countryCode, cancellationToken);
            }
            catch (Exception ex)
            {

                _logger.LogError($"Error!! Exception_ {ex.GetType().Name}");
                _logger.LogError($"Error!! get catalog from state with query: {ex.Message}, retry without it....");
                _logger.LogError(ex.Message);

                hotels = await _hotelCatalogService.GetHotelsWithFilter(countryCode, cancellationToken);

            }

            return Ok(hotels);
        }

        [HttpGet("{code}")]
        public async Task<ActionResult<Hotel>> Get(string code, CancellationToken cancellationToken)
        {
            await Task.Delay(10000);
            var hotel = await _hotelCatalogService.GetHotel(code, cancellationToken);
            return Ok(hotel);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Hotel hotel, CancellationToken cancellationToken)
        {
            await _hotelCatalogService.SaveOrUpdateHotel(hotel, cancellationToken);
            return Ok(hotel);
        }

        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] Hotel office)
        //{
        //}

        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}

        //"blobstorage-state"
    }
}