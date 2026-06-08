var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Middlware 01 the do nothing middlware
app.Use((RequestDelegate next) => next);

// Middlware 02 interceptor middlware means it modify or intercept the request to add or apply logic
app.Use(
    (RequestDelegate next) =>
    {
        return async (HttpContext context) =>
        {
            await context.Response.WriteAsync($"This is the Middleware 02");
            await next(context);
        };
    }
);

// Middlware 03 this is the extension method used for the easier writting and more readable code instead of wirtting it in the same way of middlware 02
app.Use(
    async (HttpContext context, RequestDelegate next) =>
    {
        await context.Response.WriteAsync($"This is the Middleware 03");
        await next(context);
    }
);
app.Run();
