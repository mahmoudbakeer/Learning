using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.VisualBasic;

namespace RestfulApi.Filters;

public class TrackActionFilterTime : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(
        ActionExecutingContext context,
        ActionExecutionDelegate next
    )
    {
        Console.WriteLine("Before the call.");

        context.HttpContext.Items["StartTime"] = DateTime.UtcNow;
        await next();
        context.HttpContext.Response.Headers.Append(
            "Elapsed-Time",
            $"{(DateTime)context.HttpContext.Items["StartTime"]! - DateTime.UtcNow}ms"
        );

        Console.WriteLine("After the call.");
    }
}
