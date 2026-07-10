using Microsoft.EntityFrameworkCore;
using RestfulApi.Controllers;
using RestfulApi.Filters;

/*
 * ==========================================================================================
 * ASP.NET CORE RESULT FILTERS
 * ==========================================================================================
 *
 * WHAT IT IS:
 * The final stage of the filter pipeline. It wraps the execution of the `IActionResult`.
 * It runs AFTER your API endpoint (Action method) finishes its work, but BEFORE the
 * framework takes your C# object and serializes it into the final JSON response stream.
 *
 * WHY WE USE IT:
 * 1. Final Response Modification: It allows you to intercept the exact C# object or
 * status code your endpoint returned and modify it before the client sees it.
 * 2. Header Injection: The cleanest place to inject dynamic HTTP response headers
 * (like Pagination metadata) based on the exact data your endpoint just produced.
 *
 * WHEN TO USE IT:
 * - Standardized API Responses: Catching raw data objects returned by an endpoint
 * and wrapping them in a standard envelope (e.g., `{ data: [...], success: true }`).
 * - Pagination Headers: Injecting `X-Total-Count` or `X-Current-Page` into the HTTP
 * Response headers.
 * - Removing Data: Stripping out internal fields from a DTO right before serialization.
 *
 * WHEN NOT TO USE IT (THE TRAPS):
 * - DO NOT use it for Exception Handling! If your Action method throws an unhandled
 * error, the Result Filter is completely bypassed and ignored. (Use Exception Filters).
 * - Minimal APIs: Result filters do not natively support Minimal APIs (use Endpoint
 * Filters instead).
 *
 * HIGH-LEVEL IMPLEMENTATION STEPS:
 * 1. Define Class: Create a class implementing `IAsyncResultFilter`.
 * 2. Implement Method: `Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)`
 * 3. The "Way In": Inspect `context.Result`. If it is an `ObjectResult`, you can read
 * or replace its `.Value` property (your actual data) *before* calling `next()`.
 * 4. The Execution: Call `await next()` to let the framework serialize the JSON and
 * write it to the client's HTTP response stream.
 * 5. Register: Apply it using `[TypeFilter(typeof(MyResultFilter))]` on your Controller
 * or Action.
 * ==========================================================================================
 */
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(op =>
{
    op.Filters.Add<ResultFilter>();
});

var app = builder.Build();
app.MapControllers();

app.Run();
