var builder = WebApplication.CreateBuilder(args);

// Service Lifetimes in ASP.NET Core
//
// Singleton
// - One instance for the entire application lifetime.
// - Shared across all requests and consumers.
// - Use for stateless, thread-safe services, application-wide caches,
//   configuration providers, or expensive-to-create services.
//
// Scoped
// - One instance per HTTP request.
// - Shared by all consumers within the same request.
// - Use for business services, repositories, DbContext, and services
//   that manage request-specific data.
//
// Transient
// - A new instance is created every time it is requested.
// - No state is shared between consumers.
// - Use for lightweight, stateless services such as helpers,
//   validators, and data transformers.
//
// Rule of Thumb:
// - Singleton -> One instance for the entire application.
// - Scoped    -> One instance per HTTP request.
// - Transient -> One instance per service resolution.
//
// Default choice for most application/business services: Scoped.
// Use Singleton only when a shared thread-safe instance is needed.
// Use Transient for small stateless services.
var app = builder.Build();

app.MapGet(
    "/Check",
    (ServiceA ServiceA, ServiceB ServiceB) =>
    {
        return new { A = ServiceA.GetInfo(), B = ServiceB.GetInfo() };
    }
);

app.MapGet(
    "/Check2",
    (ServiceB ServiceB) =>
    {
        return ServiceB.GetInfo();
    }
);
app.Run();

public static class DependecyInjection
{
    public static IServiceCollection AddWeatherServices(this IServiceCollection services)
    {
        services.AddScoped<ServiceA>();
        services.AddScoped<ServiceB>();
        services.AddSingleton<ServiceB>(); // make changes here to see how the response will be different between the request or on the same request
        return services;
    }
}

public class RequestTracker
{
    public string TrackerId = Guid.NewGuid().ToString()[..8];
}

public class ServiceA(RequestTracker requestTracker)
{
    public string GetInfo()
    {
        return $"the TrackerId = {requestTracker.TrackerId}";
    }
}

public class ServiceB(RequestTracker requestTracker)
{
    public string GetInfo()
    {
        return $"the TrackerId = {requestTracker.TrackerId}";
    }
}
