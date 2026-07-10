using DeveloperExceptionPage.Endpoints;

/*
 * ==========================================================================================
 * ASP.NET CORE IExceptionHandler (.NET 8+ MODERN EXCEPTION HANDLING)
 * ==========================================================================================
 *
 * WHAT IT IS:
 * Introduced in .NET 8, `IExceptionHandler` is the modern, built-in interface for
 * intercepting and formatting unhandled exceptions directly within the middleware
 * pipeline. It completely replaces the legacy approach of re-routing to an
 * ErrorController or writing clunky inline middleware delegates.
 *
 * WHY WE USE IT (THE UPGRADE):
 * 1. Performance: It avoids executing the routing engine a second time just to
 * find a "/error" endpoint. It handles the response right where the crash happened.
 * 2. Chain of Responsibility: You can register MULTIPLE handlers. For example, you can
 * have a `ValidationExceptionHandler` and a `GlobalExceptionHandler`. ASP.NET Core runs
 * them in order. If one returns `true`, the pipeline stops. If `false`, it moves to the next.
 * 3. Perfect ProblemDetails Integration: It seamlessly writes RFC 9457 compliant JSON
 * directly to the HTTP response stream.
 *
 * WHEN TO USE IT:
 * - In every modern .NET 8+ API for global, application-wide error formatting and logging.
 *
 * WHEN NOT TO USE IT:
 * - You are stuck on .NET 6 or 7 (you must use the ErrorController or custom middleware).
 * - For expected business logic flow. (Do not use exceptions for control flow; return
 * 400 Bad Request directly from your endpoint for normal validation failures).
 *
 * HIGH-LEVEL IMPLEMENTATION STEPS:
 * 1. Define Class: Create a class implementing `IExceptionHandler`.
 * 2. Implement Method: `ValueTask<bool> TryHandleAsync(HttpContext, Exception, CancellationToken)`
 * 3. Log the Error: Use an injected `ILogger` to safely record the crash.
 * 4. Format the Response: Create a `ProblemDetails` object, mapping the specific exception
 * type to the appropriate HTTP status code (usually 500, sometimes 400/404).
 * 5. Write to Stream: `await httpContext.Response.WriteAsJsonAsync(problemDetails);`
 * 6. Short-Circuit: Return `true` to tell the framework the error was successfully handled.
 * 7. Register (Program.cs):
 *    `builder.Services.AddExceptionHandler<GlobalExceptionHandler>();`
 *    `builder.Services.AddProblemDetails();`
 *    `app.UseExceptionHandler();` // NOTE: No route string argument needed here anymore!
 * ==========================================================================================
 */
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddProblemDetails();
var app = builder.Build();
app.UseExceptionHandler();
app.UseStatusCodePages();
app.MapErrorEndpoints();
app.Run();
