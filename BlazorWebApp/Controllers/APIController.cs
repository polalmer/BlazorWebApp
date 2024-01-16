using Microsoft.AspNetCore.Mvc;
using static BlazorWebApp.Components.Pages.Weather;

namespace BlazorWebApp.Controllers;

[Route("API/Controller")]
[ApiController]
public class APIController : ControllerBase
{
    [HttpGet("Weather")]
    public async Task<List<WeatherForecast>> Get()
    {
        // Simulate asynchronous loading to demonstrate streaming rendering
        await Task.Delay(500);

        var startDate = DateOnly.FromDateTime(DateTime.Now);
        var summaries = new[] { "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching" };
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = startDate.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = summaries[Random.Shared.Next(summaries.Length)]
        }).ToList();
    }
}