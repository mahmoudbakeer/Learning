using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.VisualBasic;

namespace RestfulApi.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class TrackActionFilterTimeV3 : ActionFilterAttribute
{
    public override async Task OnActionExecutionAsync(
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
