/*
 * ==========================================================================================
 * ASP.NET CORE SECURITY: CORS (CROSS-ORIGIN RESOURCE SHARING)
 * ==========================================================================================
 *
 * WHAT IT IS:
 * Browsers enforce the Same-Origin Policy, preventing JavaScript on one domain (e.g.,
 * frontend.com) from making API calls to another domain (e.g., backend.com) to stop XSS
 * and CSRF attacks. CORS is the standard that allows servers to safely punch holes in
 * this policy.
 *
 * HOW IT WORKS:
 * The browser sends an `Origin` header. The server responds with an
 * `Access-Control-Allow-Origin` header. If they don't match, the BROWSER throws an error.
 * For complex requests (PUT, DELETE, custom headers), the browser sends an invisible
 * `OPTIONS` request first (The Preflight) to ask permission before sending the real request.
 *
 * PIPELINE PLACEMENT RULE:
 * `app.UseCors()` MUST be placed after `app.UseRouting()` but before `app.UseAuthorization()`.
 * The CORS middleware needs to know which route is being executed, but it must reply to the
 * OPTIONS preflight request before the Authorization middleware tries to demand a token!
 * ==========================================================================================
 */
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("https://localhost:7070").AllowAnyHeader().AllowAnyMethod();
    });
});
var app = builder.Build();
app.UseCors();
app.MapGet(
    "/api/bookings",
    () =>
    {
        return Results.Ok(
            new List<object>
            {
                new
                {
                    Id = Guid.NewGuid(),
                    Room = "A100",
                    CustomerName = "Alice",
                    CheckIn = new DateTime(2025, 6, 1),
                    CheckOut = new DateTime(2025, 6, 5),
                },
                new
                {
                    Id = Guid.NewGuid(),
                    Room = "A101",
                    CustomerName = "Bob",
                    CheckIn = new DateTime(2025, 6, 3),
                    CheckOut = new DateTime(2025, 6, 7),
                },
                new
                {
                    Id = Guid.NewGuid(),
                    Room = "A102",
                    CustomerName = "Charlie",
                    CheckIn = new DateTime(2025, 6, 10),
                    CheckOut = new DateTime(2025, 6, 12),
                },
                new
                {
                    Id = Guid.NewGuid(),
                    Room = "A103",
                    CustomerName = "Diana",
                    CheckIn = new DateTime(2025, 6, 8),
                    CheckOut = new DateTime(2025, 6, 9),
                },
                new
                {
                    Id = Guid.NewGuid(),
                    Room = "A104",
                    CustomerName = "Ethan",
                    CheckIn = new DateTime(2025, 6, 15),
                    CheckOut = new DateTime(2025, 6, 18),
                },
                new
                {
                    Id = Guid.NewGuid(),
                    Room = "A105",
                    CustomerName = "Fiona",
                    CheckIn = new DateTime(2025, 6, 20),
                    CheckOut = new DateTime(2025, 6, 22),
                },
                new
                {
                    Id = Guid.NewGuid(),
                    Room = "A106",
                    CustomerName = "George",
                    CheckIn = new DateTime(2025, 6, 25),
                    CheckOut = new DateTime(2025, 6, 28),
                },
                new
                {
                    Id = Guid.NewGuid(),
                    Room = "A107",
                    CustomerName = "Hannah",
                    CheckIn = new DateTime(2025, 6, 11),
                    CheckOut = new DateTime(2025, 6, 13),
                },
                new
                {
                    Id = Guid.NewGuid(),
                    Room = "A108",
                    CustomerName = "Ian",
                    CheckIn = new DateTime(2025, 6, 5),
                    CheckOut = new DateTime(2025, 6, 6),
                },
                new
                {
                    Id = Guid.NewGuid(),
                    Room = "A109",
                    CustomerName = "Judy",
                    CheckIn = new DateTime(2025, 6, 29),
                    CheckOut = new DateTime(2025, 6, 30),
                },
            }
        );
    }
);
app.Run();
