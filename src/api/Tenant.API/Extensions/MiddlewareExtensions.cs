namespace Tenant.API.Extensions;

internal static class MiddlewareExtensions
{
    internal static IApplicationBuilder UseApplicationMiddlewares(this IApplicationBuilder app)
    {
        // app.UseMiddleware<TokenCheckerMiddleware>();
        //app.UseMiddleware<RestrictAccessMiddleware>();
        //app.UseMiddleware<LogContextTraceLoggingMiddleware>();

        return app;
    }
}
