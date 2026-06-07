namespace MiddleWaresPipleline.Middlewares
{
    public class ExceptionHandelerMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await context.Response.WriteAsync($"Error : \r\n");
                await context.Response.WriteAsync($"{ex.Message}");
            }
        }
    }
}
