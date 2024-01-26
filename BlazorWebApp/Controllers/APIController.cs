using BlazorWebApp.Client.Classes;
using Microsoft.AspNetCore.Mvc;

namespace BlazorWebApp.Controllers;

[Route("API/Controller")]
[ApiController]
public class APIController(IWeatherService weatherService) : ControllerBase
{
    private readonly IWeatherService _weatherService = weatherService;

    [HttpGet("Weather")]
    public async Task<List<WeatherForecast>> Get()
    {
        return await _weatherService.GetWeatherForecastsAsync();
    }
}