using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace RestfulApi.Filters;

public class ResourceFilter(IConfiguration config) : IAsyncResourceFilter
{
    public async Task OnResourceExecutionAsync(
        ResourceExecutingContext context,
        ResourceExecutionDelegate next
    )
    {
        var tentId = context.HttpContext.Request.Headers["TenentId"].ToString();
        var apikey = context.HttpContext.Request.Headers["x-Apikey"].ToString();
        var expApi = config[$"Tenent:{tentId}:ApiKey"];

        if (string.IsNullOrEmpty(apikey) || apikey != expApi)
        {
            context.Result = new UnauthorizedResult();
            return; // short circuit
        }

        await next();
    }
}
