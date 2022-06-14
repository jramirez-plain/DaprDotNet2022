using Microsoft.AspNetCore.Mvc;

namespace Rater.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RatesController : ControllerBase
    {
        private readonly ILogger<RatesController> _logger;

        public RatesController(ILogger<RatesController> logger)
        {
            _logger = logger;
        }

        [HttpPost("/cron")]
        public Task<ActionResult> CreateRatesScheduled()
        {
            //todo: To test: In order to tell Dapr that the event was not processed correctly in your application and schedule it for redelivery, return any response other than 200 OK.For example, a 500 Error.
            //var rnd = Random.Shared.Next(0, 1);
            //return Convert.ToBoolean(rnd) ? Ok() : Conflict();
            throw new NotImplementedException();
        }

        [HttpPost("/cron")]
        public Task<ActionResult> CreateRate([FromBody]DateTime date)
        {
            //todo: To test: In order to tell Dapr that the event was not processed correctly in your application and schedule it for redelivery, return any response other than 200 OK.For example, a 500 Error.
            //var rnd = Random.Shared.Next(0, 1);
            //return Convert.ToBoolean(rnd) ? Ok() : Conflict();
            throw new NotImplementedException();

        }

        //private Task CreateRates(DateTime from, DateTime to)
        //{
        //    var range = to..from;
        //    foreach(to..)
        //}
    }
}