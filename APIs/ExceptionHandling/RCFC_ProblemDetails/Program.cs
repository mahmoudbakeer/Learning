using DeveloperExceptionPage.Endpoints;

/*
 * ==========================================================================================
 * RFC 9457 (PROBLEM DETAILS FOR HTTP APIs)
 * ==========================================================================================
 *
 * WHAT IT IS:
 * The official Internet Engineering Task Force (IETF) standard for formatting error
 * responses in HTTP APIs. It dictates that instead of returning random, custom JSON
 * structures when an error occurs, your API should return a standardized JSON object
 * with 5 specific base properties:
 * - `type`: A URI identifying the problem type (e.g., "https://tools.ietf.org/html/rfc9110#section-15.5.1").
 * - `title`: A short, human-readable summary of the problem (e.g., "One or more validation errors occurred.").
 * - `status`: The HTTP status code (e.g., 400).
 * - `detail`: A human-readable explanation specific to this exact occurrence.
 * - `instance`: A URI reference that identifies the specific occurrence (often a Trace ID).
 * * WHY WE USE IT:
 * 1. Universal Consistency: Front-end developers (React, Angular, Mobile apps) no longer
 * have to write custom logic to parse 10 different error formats from 10 different APIs.
 * 2. Framework Integration: Modern ASP.NET Core has this baked in. If you use it, the
 * framework handles the serialization and content-types (application/problem+json) for you.
 * 3. Extensibility: The standard allows you to add custom properties (like an array of
 * specific `errors`) without breaking the core contract.
 *
 * WHEN TO USE IT:
 * - Every single time your API returns a 4xx (Client Error) or 5xx (Server Error) response
 * that includes a body. It is the gold standard for modern REST APIs.
 *
 * WHEN NOT TO USE IT:
 * - Legacy Clients: If you are supporting an old SOAP client or a legacy system that
 * explicitly expects a specific custom XML/JSON format.
 * - gRPC / Non-HTTP protocols.
 *
 * HIGH-LEVEL IMPLEMENTATION STEPS (IN .NET 7/8+):
 * 1. Global Setup (Program.cs): Call `builder.Services.AddProblemDetails();` so the
 * framework automatically formats 404s, 401s, etc., into RFC 9457 format.
 * 2. Manual Endpoints (Minimal APIs): Use `return Results.Problem(detail: "Item not found", statusCode: 404);`
 * 3. Manual Endpoints (Controllers): Use `return Problem(...)` or `return ValidationProblem(...)`.
 * 4. Exception Handling: Inside your global `IExceptionHandler`, catch unhandled crashes
 * and return a `ProblemDetails` object so even 500 crashes follow the standard.
 * ==========================================================================================
 */

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error");
}
else
{
    app.UseExceptionHandler("/development-error");
}
app.MapControllers();
app.MapErrorEndpoints();
app.Run();
