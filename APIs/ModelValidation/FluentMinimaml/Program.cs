using System.Text.Json.Serialization;
using ControllerDataAnnotation.Requests;
using FluentValidation;
using FluentValidation.AspNetCore;
using MinimalFluentValidation.ProductRequestValidatorFilter;

/*
 * ==========================================================================================
 * FLUENT VALIDATION ENDPOINT FILTER (MINIMAL APIs)
 * ==========================================================================================
 *
 * WHAT IT IS:
 * A generic Minimal API interceptor (`IEndpointFilter`) that automatically validates
 * incoming requests using FluentValidation before they are allowed to hit the endpoint.
 *
 * THE "HOLY TRINITY" OF MODERN VALIDATION (HOW IT WORKS):
 * 1. The Bouncer (`IEndpointFilter`): Stops the HTTP request and extracts the DTO
 * from the incoming arguments.
 * 2. The Rulebook (`IValidator<T>`): Reaches into the Dependency Injection container
 * (`HttpContext.RequestServices`) to find the specific FluentValidation rules
 * registered for this exact DTO.
 * 3. The Fail Ticket (`Results.ValidationProblem`): If the rules fail, it completely
 * bypasses the endpoint and instantly returns a beautifully formatted, RFC 9457
 * compliant 400 Bad Request JSON response.
 *
 * WHY WE USE IT:
 * - 100% Clean Lambdas: Your Minimal API endpoints will never contain `if(!isValid)`
 * checks again. If the code inside your endpoint is executing, you are mathematically
 * guaranteed that the data is valid.
 * - Global Consistency: Every validation failure in the entire application will return
 * the exact same JSON structure to the front-end team.
 *
 * HIGH-LEVEL IMPLEMENTATION STEPS (PROGRAM.CS):
 * 1. Register Validators: `builder.Services.AddValidatorsFromAssemblyContaining<Program>();`
 * 2. Attach to Endpoint:
 * `app.MapPost("/users", CreateUser)`
 * `.AddEndpointFilter<ValidationFilter<CreateUserRequest>>();`
 * ==========================================================================================
 */
var builder = WebApplication.CreateBuilder(args);
builder.Services.ConfigureHttpJsonOptions(op =>
{
    op.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
var app = builder.Build();
app.MapPost(
        "api/products",
        (ProductRequest productRequest) =>
        {
            return Results.Created($"api/products/{Guid.NewGuid}", productRequest);
        }
    )
    .AddEndpointFilter<ProductRequestValidatorFilter<ProductRequest>>();

app.Run();
