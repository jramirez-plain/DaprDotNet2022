using Dapr.Client;
using Microsoft.AspNetCore.Mvc;

namespace CapacityPlanner.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly DaprClient _daprClient;
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(DaprClient daprClient, ILogger<WeatherForecastController> logger)
        {
            _daprClient = daprClient;
            _logger = logger;
        }


        [HttpGet(Name = "GetSecrets")]        
        public async Task<IEnumerable<string>> GetSecrets()
        {

            _logger.LogTrace("Secrets Call");

            var secrets = await _daprClient.GetSecretAsync("mysecretstore", "secret");

            return secrets.Select(s => s.ToString());

        }
    }
}