using BlazorWebApp.Client.Classes;

namespace BlazorWebApp.Controllers;

public interface IWeatherService
{
    public Task<List<WeatherForecast>> GetWeatherForecastsAsync();
}
