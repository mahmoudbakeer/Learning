using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace RestfulApi.Filters;

public class ExceptionFilter : IAsyncExceptionFilter
{
    public Task OnExceptionAsync(ExceptionContext context)
    {
        var problemdetails = new ProblemDetails()
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "Internal Server Error",
            Detail = context.Exception.Message,
        };

        context.Result = new ObjectResult(problemdetails) { StatusCode = problemdetails.Status };
        context.ExceptionHandled = true;
        return Task.CompletedTask;
    }
}
