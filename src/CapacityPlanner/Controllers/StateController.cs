using Dapr.Client;
using Microsoft.AspNetCore.Mvc;

namespace CapacityPlanner.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StateController : ControllerBase
    {
        private readonly DaprClient _daprClient;
        private readonly ILogger<StateController> _logger;

        private const string STATESTORE = "mystatestore";

        public StateController(
            DaprClient daprClient,
            ILogger<StateController> logger)
        {
            _daprClient = daprClient;
            _logger = logger;
        }


        [HttpGet]
        public async Task<IActionResult> Get(string key)
        {
            var value = await _daprClient.GetStateAsync<string>(STATESTORE, key);

            _logger.LogTrace($"Read from state store: key {key} - value {value}");

            return Ok(value);
        }
    }
}