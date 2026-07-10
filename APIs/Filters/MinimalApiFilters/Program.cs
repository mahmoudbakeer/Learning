using Microsoft.EntityFrameworkCore;
using RestfulApi.Filters;

/*
 * ==========================================================================================
 * ASP.NET CORE ENDPOINT FILTERS (MINIMAL APIs)
 * ==========================================================================================
 *
 * WHAT IT IS:
 * Introduced in .NET 7, this is the ONE unified filter interface specifically designed
 * for Minimal APIs. Because Minimal APIs bypass the traditional MVC Controller pipeline
 * to maximize performance, traditional Action, Resource, and Result filters DO NOT WORK here.
 * IEndpointFilter replaces all three of them in a single, streamlined interface.
 *
 * WHY WE USE IT:
 * 1. The "All-in-One" Wrapper: Code you write *before* calling next() acts like an
 * Action/Resource filter. Code you write *after* calling next() acts like a Result filter.
 * 2. Keep Lambdas Clean: Minimal API lambdas can get messy fast. Endpoint filters allow
 * you to extract validation, logging, and caching out of the endpoint routing file.
 *
 * WHEN TO USE IT:
 * - Request Validation: Intercepting the parsed C# DTO to validate it (e.g., using
 * FluentValidation) before the endpoint executes.
 * - Endpoint-Specific Logging: Logging the exact parameters submitted to a specific route.
 * - Response Formatting: Wrapping the endpoint's return value in a standard JSON envelope.
 *
 * WHEN NOT TO USE IT (THE TRAPS):
 * - DO NOT use it for Exceptions! Minimal APIs do not support exception filters. Use
 * global Exception Handling Middleware instead.
 * - DO NOT use it for Authorization! Use the `.RequireAuthorization("Policy")` extension
 * method directly on the endpoint instead.
 * - MVC Controllers: If you are using traditional Controllers, stick to Action Filters.
 *
 * HIGH-LEVEL IMPLEMENTATION STEPS:
 * 1. Define Class: Create a class implementing `IEndpointFilter`.
 * 2. Implement Method: `ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)`
 * 3. The "Way In" (Action Filter): Inspect `context.Arguments` to read the incoming DTOs.
 * 4. The Short-Circuit: To abort (e.g., validation failed), `return Results.BadRequest(errors);`
 * and DO NOT call `next()`.
 * 5. The Execution: Call `var result = await next(context);` to run the actual endpoint.
 * 6. The "Way Out" (Result Filter): Inspect or modify the `result` object.
 * 7. Register: Attach it to your endpoint or RouteGroup using `.AddEndpointFilter<MyFilter>()`.
 * ==========================================================================================
 */

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.MapGet(
        "api/products",
        () =>
        {
            return Results.Ok(new { Name = "Mahmoud", Price = 29.99m });
        }
    )
    .AddEndpointFilter<ResultFilter>()
    .AddEndpointFilter<TimeTrackerFilter>();

app.Run();
