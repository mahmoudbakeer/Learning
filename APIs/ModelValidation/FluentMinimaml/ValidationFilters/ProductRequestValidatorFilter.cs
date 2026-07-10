using System.ComponentModel.DataAnnotations;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MinimalFluentValidation.ProductRequestValidatorFilter;

public class ProductRequestValidatorFilter<T> : IEndpointFilter
    where T : class
{
    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext context,
        EndpointFilterDelegate next
    )
    {
        var validator = context.HttpContext.RequestServices.GetService<IValidator<T>>();
        var model = context.Arguments.OfType<T>().FirstOrDefault();

        if (model is null || validator is null)
        {
            return Results.Problem(
                new ProblemDetails
                {
                    Title = "Bad Request.",
                    Status = StatusCodes.Status400BadRequest,
                    Detail = $"{nameof(T)} cannot be null or empty.",
                }
            );
        }

        var validationResults = await validator.ValidateAsync(model);

        if (!validationResults.IsValid)
        {
            var groupedresults = validationResults
                .Errors.GroupBy(vr => vr.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(vr => vr.ErrorMessage).ToArray());

            return Results.ValidationProblem(groupedresults);
        }
        return await next(context);
    }
}
