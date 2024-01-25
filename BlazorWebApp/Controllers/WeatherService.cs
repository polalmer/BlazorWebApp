using BlazorWebApp.Client.Classes;

namespace BlazorWebApp.Controllers;

public static class WeatherService
{
    private static readonly List<string> summaries = ["Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"];

    public static async Task<List<WeatherForecast>> GetWeatherForecastsAsync()
    {
        await Task.Delay(500);

        var startDate = DateOnly.FromDateTime(DateTime.Now);
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = startDate.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = summaries[Random.Shared.Next(summaries.Count)]
        }).ToList();
    }
}
