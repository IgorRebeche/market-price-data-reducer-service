using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace market_price_reducer_service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IOptions<List<Timeframe>> _timeFrameOptions;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IOptions<List<Timeframe>> timeFrameOptions)
        {
            _logger = logger;
            _timeFrameOptions = timeFrameOptions;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            _logger.LogInformation("Test");
            _logger.LogInformation("{@object}", _timeFrameOptions.Value);
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}