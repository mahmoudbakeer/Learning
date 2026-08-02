using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace MinimalProjectManagement.Filters;


public class ValidationFilter<T> : IEndpointFilter where T : class
{
    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext context,
        EndpointFilterDelegate next)
    {

        var validator = context.HttpContext.RequestServices.GetService<IValidator<T>>(); // bring the fluent validator.

        var model = context.Arguments.OfType<T>().FirstOrDefault();

        if (validator is null || model is null)
        {
            return Results.Problem(new ProblemDetails
            {
                Title = "Bad Request",
                Status = StatusCodes.Status400BadRequest,
                Detail = $"{nameof(T)} is null"
            });
        }

        var result = await validator.ValidateAsync(model);

        if (!result.IsValid)
        {
            var errors = result.Errors.GroupBy(g => g.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(p => p.ErrorMessage!).ToArray());

            return Results.ValidationProblem(errors);
        }

        return await next(context);
    }
}