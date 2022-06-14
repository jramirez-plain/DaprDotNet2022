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


        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
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