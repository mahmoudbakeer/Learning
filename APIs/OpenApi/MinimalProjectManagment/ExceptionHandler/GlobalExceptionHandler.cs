using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace ControllerProjectManagement.ExceptionHandler;

public class GlobalExceptionHandler(IProblemDetailsService problemDetailsService, IHostEnvironment environment) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        httpContext.Response.StatusCode = exception switch
        {
            ValidationException => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError,
        };

        return await problemDetailsService.TryWriteAsync(context: new ProblemDetailsContext
        {
            HttpContext = httpContext,
            ProblemDetails = new ProblemDetails
            {
                Title = exception.Message,
                Status = httpContext.Response.StatusCode,
                // Only include the stack trace if you are in a development environment
                Detail = environment.IsDevelopment() ? exception.StackTrace : "An unexpected error occurred.",
                Type = exception.GetType().Name
            }
        });
    }
}