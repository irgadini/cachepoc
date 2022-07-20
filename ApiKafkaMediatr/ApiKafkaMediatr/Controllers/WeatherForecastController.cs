using CacheLib;
using Microsoft.AspNetCore.Mvc;
using Polly;
using Polly.Caching;

namespace ApiKafkaMediatr.Controllers
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
        private readonly ICachePolicyFactory _cachePolicy;


        public WeatherForecastController(ICachePolicyFactory cachePolicy, ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
            _cachePolicy = cachePolicy;
            
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            AsyncCachePolicy<IEnumerable<WeatherForecast>> _cache = _cachePolicy.Cache<IEnumerable<WeatherForecast>>("WeatherForecast");
            Context context = new("WeatherForecast");

            var retorno = await _cache.ExecuteAsync((context) => Method(), context);
            return retorno;
        }

        private async Task<IEnumerable<WeatherForecast>> Method()
        {
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