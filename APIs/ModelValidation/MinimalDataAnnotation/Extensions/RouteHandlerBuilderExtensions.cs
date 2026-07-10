using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace MinimalDataAnnotations.Extensions;

public static class RouteHandlerBuilderExtensions
{
    // Extension method that adds automatic Data Annotation validation
    // to a Minimal API endpoint.
    public static RouteHandlerBuilder Validate<T>(this RouteHandlerBuilder builder)
    {
        // Add an Endpoint Filter that executes before the endpoint handler.
        builder.AddEndpointFilter(
            async (context, next) =>
            {
                // Find the first endpoint argument of type T.
                // Example:
                // app.MapPost("/", (ProductDto dto) => { ... })
                // 'dto' will be found here.
                var argument = context.Arguments.OfType<T>().FirstOrDefault();

                // If no argument of type T exists, return 400 Bad Request.
                if (argument is null)
                {
                    return Results.Problem(
                        new ProblemDetails
                        {
                            Title = "Bad Request",
                            Status = StatusCodes.Status400BadRequest,
                            Detail = $"{nameof(T)} not found or null",
                        }
                    );
                }

                // This list will be populated by TryValidateObject()
                // with every validation error that occurs.
                List<ValidationResult> validationResults = [];

                // Validate the object using its Data Annotation attributes.
                //
                // Parameters:
                // 1. argument                -> object to validate
                // 2. ValidationContext       -> contains metadata about the object
                // 3. validationResults       -> receives all validation failures
                // 4. true                    -> validate all property attributes
                var isValid = Validator.TryValidateObject(
                    argument,
                    new ValidationContext(argument),
                    validationResults,
                    true
                );

                // If one or more validation errors occurred...
                if (!isValid)
                {
                    // Convert
                    //
                    // List<ValidationResult>
                    //
                    // into
                    //
                    // Dictionary<string,string[]>
                    //
                    // because Results.ValidationProblem() expects this format.
                    var errorGroups = validationResults
                        // A ValidationResult may belong to multiple properties.
                        // Example:
                        // MemberNames = ["Password", "ConfirmPassword"]
                        //
                        // SelectMany flattens them into one sequence.
                        .SelectMany(v =>
                            (v.MemberNames.Any() ? v.MemberNames : new[] { "" })
                            // Create an object containing
                            // - Property name
                            // - Error message
                            .Select(name => new { name, v.ErrorMessage })
                        )
                        // Group all errors belonging to the same property.
                        //
                        // Example:
                        //
                        // Name
                        //  - Required
                        //  - MinLength
                        //
                        // Price
                        //  - Range
                        .GroupBy(g => g.name)
                        // Convert each group into:
                        //
                        // Key   -> Property Name
                        // Value -> Array of error messages
                        //
                        // Result:
                        //
                        // {
                        //     "Name":
                        //     [
                        //         "Required",
                        //         "Minimum length is 3"
                        //     ],
                        //
                        //     "Price":
                        //     [
                        //         "Must be between 1 and 1000"
                        //     ]
                        // }
                        .ToDictionary(g => g.Key, g => g.Select(x => x.ErrorMessage!).ToArray());

                    // Return a standard ASP.NET Core validation response (HTTP 400).
                    return Results.ValidationProblem(errorGroups);
                }

                // Validation succeeded.
                // Continue executing the endpoint handler.
                return await next(context);
            }
        );

        return builder;
    }
}
