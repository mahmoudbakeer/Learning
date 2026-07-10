using System.Text.Json.Serialization;
using FluentValidation;
using FluentValidation.AspNetCore;

/*
 * ==========================================================================================
 * FLUENT VALIDATION (THE MODERN ENTERPRISE STANDARD)
 * ==========================================================================================
 *
 * WHAT IT IS:
 * A wildly popular open-source .NET library that uses a fluent interface and lambda
 * expressions to build strongly-typed validation rules. Instead of putting [Attributes]
 * directly on your properties, you create a dedicated, separate class to hold the rules.
 *
 * WHY WE USE IT (THE UPGRADE OVER DATA ANNOTATIONS):
 * 1. True Separation of Concerns: Your DTOs and Domain Models go back to being clean,
 * pure C# objects (POCOs) with zero validation logic cluttering them up.
 * 2. Cross-Property Validation: It is incredibly easy to write rules that compare two
 * properties (e.g., `RuleFor(x => x.EndDate).GreaterThan(x => x.StartDate)`).
 * 3. Conditional Validation: You can easily say, "Only validate the State property IF
 * the Country property is 'USA'." Doing this with built-in attributes is a nightmare.
 * 4. Unit Testing: Because validators are just normal C# classes, you can unit test
 * your business rules instantly without spinning up an ASP.NET Core test server.
 *
 * WHEN TO USE IT:
 * - In almost every modern .NET API, especially if you are using Clean Architecture,
 * CQRS (MediatR), or Minimal APIs.
 *
 * HIGH-LEVEL IMPLEMENTATION STEPS:
 * 1. Install: Add the `FluentValidation.DependencyInjectionExtensions or FluentValidation.AspNetCore` NuGet package.
 * 2. Define Class: Create a class that inherits from `AbstractValidator<T>`.
 * 3. Write Rules: Inside the constructor, use `RuleFor(x => x.Prop)...` to chain rules.
 * 4. Register: In Program.cs, call `builder.Services.AddValidatorsFromAssembly(...)` NOTE : Try to add the typeof(ValidatorClassYouCreated).Assembly.
 * 5. Execute:
 * - (MVC): It can automatically hook into the ModelState.
 * - (Minimal APIs): Inject the `IValidator<T>` into an `IEndpointFilter` to
 * automatically validate requests before they hit your endpoints.
 * - (CQRS/MediatR): Hook it into a MediatR Pipeline Behavior to validate all Commands.
 * ==========================================================================================
 */
var builder = WebApplication.CreateBuilder(args);
builder
    .Services.AddControllers()
    .AddJsonOptions(op =>
    {
        op.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
var app = builder.Build();
app.MapControllers();

app.Run();
