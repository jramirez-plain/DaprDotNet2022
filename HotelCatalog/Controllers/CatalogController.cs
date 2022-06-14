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
        public Task<ActionResult<IEnumerable<Office>>> Get(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        [HttpGet("{id}")]
        public Office Get(int id)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public void Post([FromBody] Office office)
        {
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Office office)
        {
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}