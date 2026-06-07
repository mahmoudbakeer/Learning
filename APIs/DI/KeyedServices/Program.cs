using Microsoft.Extensions.DependencyInjection.Extensions;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // register a group of services for more readable and cleaner code and more managable and testable one.
        builder.Services.AddServices();
        var app = builder.Build();

        app.MapGet(
            "/V1",
            ([FromKeyedServices("V1")] IServiceSomething service) =>
            {
                return service.Something();
            }
        );

        app.MapGet(
            "/V1",
            ([FromKeyedServices("V1")] IServiceSomething service) =>
            {
                return service.Something();
            }
        );
        app.Run();
    }
}

public static class DependecyInjection
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        // now lets register using the keyed registeration, so we can access the service using specific key
        services.AddKeyedTransient<IServiceSomething, ServiceSomethingV1>("V1");
        services.AddKeyedTransient<IServiceSomething, ServiceSomethingV2>("V2");
        return services;
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
