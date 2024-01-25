using BlazorWebApp.Client.Classes;
using Microsoft.AspNetCore.Mvc;

namespace BlazorWebApp.Controllers;

[Route("API/Controller")]
[ApiController]
public class APIController : ControllerBase
{
    [HttpGet("Weather")]
    public async Task<List<WeatherForecast>> Get()
    {
        return await WeatherService.GetWeatherForecastsAsync();
    }
}