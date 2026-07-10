namespace RestfulApi.Filters;

public class TimeTrackerFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext context,
        EndpointFilterDelegate next
    )
    {
        var starttime = DateTime.UtcNow;
        var result = await next(context);
        var elapsedtime = starttime - DateTime.UtcNow;

        context.HttpContext.Response.Headers.Append("ElapsedTime", $"{elapsedtime}ms");

        return result;
    }
}
