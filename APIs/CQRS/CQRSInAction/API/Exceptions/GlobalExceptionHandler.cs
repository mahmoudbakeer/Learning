
using CQRSInAction.Application.Common.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace CQRSInAction.API.Exceptions;



public sealed class GlobalExceptionHandler(IProblemDetailsService problemDetailsService) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var problem = exception switch
        {
            ValidationException ex => new ValidationProblemDetails(
                        ex.Errors.GroupBy(ex => ex.PropertyName)
                            .ToDictionary(
                                        ex => ex.Key,
                                        g => g.Select(g => g.ErrorMessage).ToArray()
                                        )
                        )
            {
                Title = "Validation Failed!",
                Status = StatusCodes.Status400BadRequest
            },
            NotFoundException ex => new ProblemDetails()
            {
                Title = "Not Found!",
                Detail = exception.Message,
                Status = StatusCodes.Status404NotFound
            },
            _ => new ProblemDetails()
            {
                Title = "Server Failed!",
                Status = StatusCodes.Status500InternalServerError,
                Detail = "An unhandled exception happened."
            },
        };


        httpContext.Response.StatusCode = problem.Status!.Value;


        await problemDetailsService.WriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            ProblemDetails = problem,

        });


        return true;

    }
}