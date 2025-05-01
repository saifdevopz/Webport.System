namespace System.API.Middlewares;

internal static class MiddlewareExtensions
{
    internal static IApplicationBuilder UseLogContextTraceLogging(this IApplicationBuilder app)
    {
        //app.UseMiddleware<RestrictAccessMiddleware>();
        //app.UseMiddleware<LogContextTraceLoggingMiddleware>();

        return app;
    }
}
