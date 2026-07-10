using System.Text.Json.Serialization;
using ControllerDataAnnotation.Requests;
using MinimalDataAnnotations.Extensions;

/*
 * ==========================================================================================
 * ASP.NET CORE DATA ANNOTATIONS ENGINE (UNDER THE HOOD)
 * ==========================================================================================
 *
 * WHAT IT IS:
 * The underlying engine that powers ASP.NET Core's automatic 400 Bad Request responses.
 * While the framework usually hides this from you, understanding these three classes is
 * required if you ever need to validate data in background workers, console apps, or
 * custom validation attributes.
 *
 * THE 3 CORE COMPONENTS:
 * 1. ValidationContext (The Environment)
 * - Wraps the DTO being validated.
 * - Holds a reference to the Dependency Injection container (`context.GetService()`).
 * - Contains an `Items` dictionary to pass "secret" data to your validation rules.
 *
 * 2. Validator (The Execution Engine)
 * - A static helper class (`Validator.TryValidateObject(...)`).
 * - It scans the object for attributes like [Required] and executes them one by one.
 *
 * 3. ValidationResult (The Outcome)
 * - The "Fail Ticket". If a rule fails, it creates one of these.
 * - Contains the `ErrorMessage` (for the user) and the `MemberNames` (the exact
 * property that failed, so the UI knows what to highlight in red).
 *
 * WHEN TO ACTUALLY USE THEM MANUALLY:
 * - Inside Custom Validation Attributes (You receive the Context, you return a Result).
 * - In Background Services (RabbitMQ/Kafka consumers) where there is no HTTP pipeline
 * to automatically validate your incoming JSON messages for you.
 */
var builder = WebApplication.CreateBuilder(args);
builder.Services.ConfigureHttpJsonOptions(op =>
{
    op.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
var app = builder.Build();
app.MapPost(
        "api/products",
        (ProductRequest productRequest) =>
        {
            return Results.Created($"api/products/{Guid.NewGuid}", productRequest);
        }
    )
    .Validate<ProductRequest>();

app.Run();
