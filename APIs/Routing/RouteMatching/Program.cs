var builder = WebApplication.CreateBuilder(args);

// WHAT IS ROUTING?
//
// Routing is the process of finding which endpoint should handle an incoming
// HTTP request.
//
// An endpoint can be:
//   - A Controller Action
//   - A Minimal API endpoint (MapGet, MapPost, ...)
//   - A Razor Page
//   - A SignalR Hub
//   - Any executable request handler
//
// Every endpoint registered in the application contains metadata such as:
//   - URL pattern (Route Template)
//   - HTTP Method (GET, POST, PUT, DELETE, ...)
//   - Authorization requirements
//   - CORS policies
//   - Other endpoint-specific information
//
// Example:
//
//   app.MapGet("/users/{id}", ...);
//
// The endpoint stores the template:
//
//   /users/{id}
//
// If a request comes for:
//
//   GET /users/15
//
// The routing system compares the request path against all registered
// endpoints. When it finds a matching template, it extracts route values:
//
//   id = 15
//
// and stores the selected endpoint and its metadata inside HttpContext.
//
// IMPORTANT:
// UseRouting() DOES NOT execute the endpoint.
// It only SELECTS and STORES the best matching endpoint.
builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

app.MapGet(
    "/Products",
    () =>
    {
        return Results.Ok(new[] { "Product #1", "Product #2" });
    }
);

app.MapGet(
    "/Route-Table",
    (IServiceProvider sp) =>
    {
        var endpoints = sp.GetRequiredService<EndpointDataSource>()
            .Endpoints.Select(sp => sp.DisplayName);

        return Results.Ok(endpoints);
    }
);
app.Run();
