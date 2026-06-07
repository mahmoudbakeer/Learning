using Microsoft.Extensions.DependencyInjection.Extensions;

// Use a Factory when the creation of a service requires custom logic
// that cannot be handled by the default ASP.NET Core DI container.
//
// Common scenarios:
// - The implementation is chosen dynamically at runtime.
// - Service creation depends on configuration values.
// - Complex initialization is required before the service is usable.
// - Constructor parameters must be computed or retrieved manually.
// - Different implementations are needed for different environments,
//   tenants, or business rules.
// - You want to encapsulate object creation and keep consumers unaware
//   of the instantiation details.
//
// If a service can be created directly through constructor injection,
// registering it normally with AddTransient, AddScoped, or AddSingleton
// is usually the simpler and preferred approach.
internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddServices();
        var app = builder.Build();

        app.MapGet(
            "/Payment/{amount}",
            (decimal amount, IPayment payment) =>
            {
                return payment.PaymentMethod(amount);
            }
        );
        app.Run();
    }
}

public static class DependecyInjection
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddTransient<IPayment>(sp =>
        {
            var config = sp.GetRequiredService<IConfiguration>();

            return (config["PaymentProvider"] == "Stripe")
                ? new StripePayment()
                : new PaypalPayment();
        });
        return services;
    }
}

interface IPayment
{
    string PaymentMethod(decimal amount);
}

public class StripePayment : IPayment
{
    public string PaymentMethod(decimal amount)
    {
        return $"payment of amount {amount}$ have been processed successfully using Stripe.";
    }
}

public class PaypalPayment : IPayment
{
    public string PaymentMethod(decimal amount)
    {
        return $"payment of amount {amount}$ have been processed successfully using Paypal.";
    }
}
