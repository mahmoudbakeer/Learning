using Microsoft.EntityFrameworkCore;
using RestfulApi.Controllers;
using RestfulApi.Filters;

/*
 * ==========================================================================================
 * ASP.NET CORE EXCEPTION FILTERS
 * ==========================================================================================
 *
 * WHAT IT IS:
 * A specialized filter that ONLY triggers if an unhandled exception (a crash) is thrown
 * during Model Binding, Action Filters, or inside your actual Action Method. It acts as
 * a safety net to catch errors before they bubble up and crash the web server.
 *
 * WHY WE USE IT:
 * 1. Clean Controllers: Completely eliminates the need to write `try-catch` blocks
 * inside every single API endpoint.
 * 2. Standardized Error Responses: Instead of ASP.NET Core sending a raw stack trace
 * or an ugly HTML 500 error page to the client, you can format the error into a clean,
 * standardized JSON payload (e.g., the RFC 7807 ProblemDetails format).
 *
 * THE BIG DEBATE: EXCEPTION FILTER VS. EXCEPTION MIDDLEWARE
 * Microsoft officially recommends using global Exception MIDDLEWARE (UseExceptionHandler)
 * over Exception FILTERS for general error handling.
 * WHY? Because Exception Filters ONLY catch errors inside the MVC Controller pipeline.
 * If a database error happens inside your Authentication Middleware or your Routing
 * Middleware before the Controller is even reached, the Exception Filter is completely
 * blind to it and your app will crash.
 *
 * WHEN TO ACTUALLY USE IT:
 * - Context-Aware Errors: When your error handling specifically needs to know WHICH
 * Controller or Action caused the error (since Middleware doesn't have MVC context).
 * - Domain-Specific Mapping: Catching custom domain exceptions (e.g., `ProductOutOfStockException`)
 * and translating them to a 400 Bad Request right at the controller level.
 *
 * WHEN NOT TO USE IT:
 * - Global App-Wide Safety Nets: Stick to Global Exception Handling Middleware.
 * - Minimal APIs: Minimal APIs do not support Exception Filters natively.
 *
 * HIGH-LEVEL IMPLEMENTATION STEPS:
 * 1. Define Class: Create a class implementing `IAsyncExceptionFilter`.
 * 2. Implement Method: `Task OnExceptionAsync(ExceptionContext context)`
 * 3. Evaluate the Error: Look at `context.Exception` to see what broke (e.g., is it a
 * `SqlException` or a `NullReferenceException`?).
 * 4. Create the Response: Build a structured JSON object containing the error details.
 * 5. Handle It: Set `context.Result = new ObjectResult(myJson) { StatusCode = 500 };`
 * AND explicitly set `context.ExceptionHandled = true;` so the app knows you fixed it.
 * 6. Register: Usually applied globally in Program.cs -> opt.Filters.Add<MyExceptionFilter>();
 * ==========================================================================================
 */

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(op =>
{
    op.Filters.Add<ExceptionFilter>();
});

var app = builder.Build();
app.MapControllers();

app.Run();
