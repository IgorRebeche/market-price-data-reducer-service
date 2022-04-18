using Application.UseCases.CreateCandlestick;
using Domain.Options;
using Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace WebApi.Controllers
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
        private readonly ICreateCandleStickUseCase _createCandleStickUseCase;
        private readonly ITickerRepository _tickerRepository;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IOptions<List<Timeframe>> timeFrameOptions, ICreateCandleStickUseCase createCandleStickUseCase)
        {
            _logger = logger;
            _timeFrameOptions = timeFrameOptions;
            _createCandleStickUseCase = createCandleStickUseCase;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IActionResult> Get()
        {
            _logger.LogInformation("Test");
            //var ticker = await _tickerRepository.GetTickersRange("Binance", "BTCUSDT", "10M",1649717930297, 1649718027551);

            //_logger.LogInformation("Tickers {@tickers}", ticker);
            //_logger.LogInformation("Tickers Count {tickers}", ticker.Count());
            await _createCandleStickUseCase.ExecuteAsync();
            var a = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            });

            return Ok(a); 
        }
    }
}