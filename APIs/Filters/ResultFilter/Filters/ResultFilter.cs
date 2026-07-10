using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace RestfulApi.Filters;

public class ResultFilter : IAsyncResultFilter
{
    public async Task OnResultExecutionAsync(
        ResultExecutingContext context,
        ResultExecutionDelegate next
    )
    {
        if (context.Result is ObjectResult objectResult && objectResult.Value is not null)
        {
            var wrapped = new { success = true, data = objectResult.Value };

            context.Result = new JsonResult(wrapped) { StatusCode = objectResult.StatusCode };
        }
        await next();
    }
}
