using Microsoft.EntityFrameworkCore;
using RestfulApi.Controllers;
using RestfulApi.Filters;

/*
 * ==========================================================================================
 * ASP.NET CORE RESOURCE FILTERS
 * ==========================================================================================
 *
 * WHAT IT IS:
 * The second filter in the ASP.NET Core pipeline. It runs immediately after Authorization,
 * but BEFORE Model Binding and before the Action Filter. It is the very first filter
 * that has access to the full MVC/Endpoint context, and the last filter to run on the
 * way out of the pipeline.
 *
 * WHY WE USE IT:
 * 1. Short-Circuiting (Performance): Because it runs before Model Binding, it is the
 * absolute best place to implement Caching. If the filter finds a cached response,
 * it can return it immediately—completely skipping the Action Filter, the Controller,
 * and the Database, saving massive amounts of CPU.
 * 2. Early Request Manipulation: You can alter incoming request data, headers, or
 * Value Providers before the framework even attempts to bind the JSON body to your C# DTOs.
 *
 * WHEN TO USE IT:
 * - Caching Responses (e.g., returning a saved JSON string if the data hasn't changed).
 * - Feature Flags (e.g., returning a 404 immediately if a whole API feature is disabled).
 * - Advanced Rate Limiting/Quotas (Stop heavy requests before the server spends time
 * parsing the complex JSON body).
 *
 * WHEN NOT TO USE IT (THE TRAPS):
 * - DO NOT use it for Model Validation! Resource Filters run *before* the C# model is
 * populated. (Use Action Filters for validation).
 * - DO NOT use it for Security/Identity checks. (Use Authorization Filters).
 *
 * HIGH-LEVEL IMPLEMENTATION STEPS:
 * 1. Define Class: Create a class implementing `IAsyncResourceFilter`.
 * 2. Implement Method: `Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)`
 * 3. The "Way In": Write code *before* calling `await next()` (e.g., Check the cache).
 * 4. The "Short-Circuit": To abort the request and skip the Controller, set
 * `context.Result = new OkObjectResult(data);` and DO NOT call `next()`.
 * 5. The "Way Out": Write code *after* `await next()` to execute logic right before the
 * response is sent to the client (e.g., Save the Controller's fresh output to the cache).
 * 6. Register: Apply it using `[TypeFilter(typeof(MyResourceFilter))]` on your Controller.
 * ==========================================================================================
 */
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(op =>
{
    op.Filters.Add<ResourceFilter>();
});

var app = builder.Build();
app.MapControllers();

app.Run();
