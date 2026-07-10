using Microsoft.EntityFrameworkCore;
using RestfulApi.Controllers;
using RestfulApi.Filters;

/*
 * ==========================================================================================
 * ASP.NET CORE FILTERS
 * ==========================================================================================
 *
 * WHAT IT IS:
 * Filters are hooks in the ASP.NET Core endpoint pipeline that allow you to run custom code
 * *before* or *after* specific stages of request processing.
 *
 * THE BIG DIFFERENCE (MIDDLEWARE VS. FILTERS):
 * - Middleware is the "Front Desk": It runs for every single HTTP request (images, 404s, etc.)
 * and only understands raw HTTP (headers, body text).
 * - Filters are the "Room Bouncers": They only run AFTER the router has matched an endpoint.
 * Because of this, Filters have full access to your C# context (ModelState, method
 * arguments, and the actual Controller context).
 *
 * WHY WE USE IT:
 * 1. DRY Principle (Don't Repeat Yourself): Move cross-cutting concerns (like logging,
 * input validation, or caching) out of your endpoints so you don't write them 100 times.
 * 2. Clean Controllers: Keep your API endpoints strictly focused on business logic.
 *
 * THE 5 TYPES OF FILTERS (IN ORDER OF EXECUTION):
 * 1. Authorization Filters: Runs first. Determines if the user is allowed to proceed (e.g., RBAC).
 * 2. Resource Filters: Runs after auth. Great for caching (can short-circuit the whole pipeline).
 * 3. Action Filters: Wraps the endpoint method. Can read/modify the exact C# parameters passed in.
 * 4. Exception Filters: Catches unhandled code errors and formats them into clean JSON responses.
 * 5. Result Filters: Wraps the IActionResult. Modifies HTTP headers right before sending the data.
 *
 * HOW TO IMPLEMENT & USE THEM (3 SCOPES):
 * 1. Define It: Create a class implementing a filter interface (e.g., IAsyncActionFilter).
 * * 2. Register/Apply It:
 * - GLOBAL LEVEL (Applies to all endpoints):
 * Register in Program.cs -> builder.Services.AddControllers(opt => opt.Filters.Add<MyFilter>());
 *
 * - CONTROLLER LEVEL (Applies to one whole controller):
 * Use the attribute -> [ServiceFilter(typeof(MyFilter))]
 * public class ProductsController : ControllerBase { ... }
 *
 * - ACTION LEVEL (Applies to one single endpoint):
 * Use the attribute directly above the method -> [ServiceFilter(typeof(MyFilter))]
 * ==========================================================================================
 */
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(op =>
{
    //     op.Filters.Add<TrackActionFilterTime>(); // global
});

var app = builder.Build();
app.MapControllers();

app.Run();
