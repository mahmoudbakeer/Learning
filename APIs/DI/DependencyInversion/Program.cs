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
                // this method will now rely on the abstracion not the implementation details
                IWeatherClient weatherClient = new WeatherClient();
                IWeatherService weatherService = new WeatherService(weatherClient);
                return weatherService.GetWeather(cityname);
            }
        );

        app.Run();
    }
}

public interface IWeatherService
{
    string GetWeather(string cityname);
}

public class WeatherService : IWeatherService
{
    private IWeatherClient _weatherclient;

    public WeatherService(IWeatherClient weatherClient)
    {
        _weatherclient = weatherClient;
    }

    public string GetWeather(string cityname)
    {
        return _weatherclient.GetWeather(cityname);
    }
}

public interface IWeatherClient
{
    string GetWeather(string cityname);
}

public class WeatherClient : IWeatherClient
{
    public string GetWeather(string cityname)
    {
        var http = new HttpClient();
        // simulate some logic and returnings
        return $"the weather in the city {cityname} is {Random.Shared.Next(10, 40)} C.";
    }
}
