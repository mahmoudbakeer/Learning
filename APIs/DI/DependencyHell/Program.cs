internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var app = builder.Build();

        app.MapGet(
            "/Weather/City/{cityname}",
            (string cityname) =>
            {
                var WeatherService = new WeatherService();
                return WeatherService.GetWeather(cityname);
            }
        );
        app.Run();
    }
}

public class WeatherService
{
    public string GetWeather(string cityname)
    {
        var WeatherClient = new WeatherClient();
        return WeatherClient.GetWeather(cityname);
    }
}

public class WeatherClient
{
    public string GetWeather(string cityname)
    {
        var http = new HttpClient();
        // simulate some logic and returnings
        return $"the weather in the city {cityname} is {Random.Shared.Next(10, 40)} C.";
    }
}
