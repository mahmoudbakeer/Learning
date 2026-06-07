internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // register a group of services for more readable and cleaner code and more managable and testable one.
        builder.Services.AddWeatherServices();
        var app = builder.Build();

        app.MapGet("/", () => "Hello World!");

        app.MapGet(
            "/Weather/City/{cityname}",
            (string cityname, IWeatherService weatherService) =>
            {
                return weatherService.GetWeather(cityname);
            }
        );

        app.Run();
    }
}

public static class DependecyInjection
{
    public static IServiceCollection AddWeatherServices(this IServiceCollection services)
    {
        services.AddTransient<IWeatherService, WeatherService>();
        // we can register them using the descriptors, which gives more features such as meta
        // A ServiceDescriptor is the foundational object that the .NET IoC container uses to understand a registered service.
        // It encapsulates the ServiceType (interface), the ImplementationType (concrete class), and the Lifetime
        services.Add(
            new ServiceDescriptor(
                typeof(IWeatherClient),
                typeof(WeatherClient),
                ServiceLifetime.Transient
            )
        );
        return services;
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
