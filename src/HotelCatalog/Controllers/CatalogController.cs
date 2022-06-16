using HotelCatalog.Models;
using HotelCatalog.Services;
using Microsoft.AspNetCore.Mvc;

namespace HotelCatalog.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CatalogController : ControllerBase
    {

        private readonly HotelCatalogService _hotelCatalogService;
        private readonly ILogger<CatalogController> _logger;

        public CatalogController(HotelCatalogService hotelCatalogService, ILogger<CatalogController> logger)
        {
            _logger = logger;
            _hotelCatalogService = hotelCatalogService;
        }


        [HttpGet]
        public Task<ActionResult<IEnumerable<Hotel>>> Get(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        [HttpGet("{code}")]
        public async Task<ActionResult<Hotel>> Get(string code, CancellationToken cancellationToken)
        {
            var hotel = await _hotelCatalogService.Get(code, cancellationToken);
            return Ok(hotel);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Hotel hotel, CancellationToken cancellationToken)
        {
            await _hotelCatalogService.Save(hotel, cancellationToken);
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