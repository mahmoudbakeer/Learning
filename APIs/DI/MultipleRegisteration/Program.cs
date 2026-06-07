using Microsoft.Extensions.DependencyInjection.Extensions;

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
        app.MapGet(
            "/Services/Single",
            (IServiceSomething serviceSomething) =>
            {
                // you will notice this will return ServiceSomethingV2
                // because the last Dependecy Injected int service for specific Interface will override the rest of the Dependecy Injected for that interface
                return serviceSomething.Something();
            }
        );
        app.MapGet(
            "/Services/Multiple",
            (IEnumerable<IServiceSomething> services) =>
            {
                // here we can choose between the multiple Dependecies for specific interface
                List<string> somethings = [];
                foreach (var item in services)
                {
                    somethings.Add(item.Something());
                }
                return somethings;
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
        services.AddTransient<IWeatherClient, WeatherClient>();
        // multiple registeration such this behavior will result new full service of existing one so its better to use TryAddTransient()
        // services.AddTransient<IWeatherClient, WeatherClient>();
        services.TryAddTransient<IWeatherClient, WeatherClient>();
        // to make a chain so we can register more services in same line
        // now lets seee how the application will behave under multiple services1
        services.AddTransient<IServiceSomething, ServiceSomethingV1>();
        services.AddTransient<IServiceSomething, ServiceSomethingV2>();
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

interface IServiceSomething
{
    string Something();
}

public class ServiceSomethingV1 : IServiceSomething
{
    public string Something()
    {
        return "This is the ServiceSomethingV1";
    }
}

public class ServiceSomethingV2 : IServiceSomething
{
    public string Something()
    {
        return "This is the ServiceSomethingV2";
    }
}
