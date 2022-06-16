using HotelCatalog.Models;
using Microsoft.AspNetCore.Mvc;

namespace HotelCatalog.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CatalogController : ControllerBase
    {


        private readonly ILogger<CatalogController> _logger;

        public CatalogController(ILogger<CatalogController> logger)
        {
            _logger = logger;
        }


        [HttpGet]
        public Task<ActionResult<IEnumerable<Hotel>>> Get(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        [HttpGet("{id}")]
        public Hotel Get(int id)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public void Post([FromBody] Hotel office)
        {
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Hotel office)
        {
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}