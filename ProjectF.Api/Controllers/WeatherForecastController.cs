using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using ProjectF.Api.Configurations;
using ProjectF.Api.Core.Models;

namespace ProjectF.Api.Controllers;

[Route("[controller]")]
[ApiVersion("1.0-actual")]
public class WeatherForecastController(ILogger<WeatherForecastController> logger) : BaseController
{
    private static readonly string[] Summaries =
    [
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    ];

    [HttpGet]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .Append(new WeatherForecast
            {
                Summary = ApiContext.ApiVersion.ToString()
            })
            .ToArray();
    }
}