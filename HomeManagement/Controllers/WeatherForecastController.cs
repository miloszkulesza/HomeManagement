using HomeManagement.Core.Consts;
using HomeManagement.Infrastructure.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HomeManagement.Controllers
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
        private readonly HomeManagementContext _dbContext;

        public WeatherForecastController(ILogger<WeatherForecastController> logger,
            HomeManagementContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            var canConnect = await _dbContext.Database.CanConnectAsync();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)],
                CanConnectToDb = canConnect
            })
            .ToArray();
        }
    }
}
