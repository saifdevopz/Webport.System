using Microsoft.AspNetCore.Http;

namespace Common.Storage.Middlewares
{
    public class RestrictAccessMiddleware(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            var referrer = context.Request.Headers["Referrer"].FirstOrDefault();
            if (string.IsNullOrEmpty(referrer))
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("Hmm, Can't reach this page");
            }
            else
            {
                await next(context);
            }
        }
    }
}
