using Common.Application.CQRS;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using System.Diagnostics;

namespace Common.Application.Behaviors;

public class LoggingBehavior<TRequest, TResponse>(
    ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
{
    public async Task<TResponse> Handle(
        TRequest request,
        Func<Task<TResponse>> nextHandler,
        CancellationToken cancellationToken = default)
    {
        string moduleName = GetModuleName(typeof(TRequest).FullName!);
        string requestName = typeof(TRequest).Name;

        // Adds tracing tags to the current Activity for distributed tracing tools
        Activity.Current?.SetTag("request.module", moduleName);
        Activity.Current?.SetTag("request.name", requestName);

        using (LogContext.PushProperty("Module", moduleName))
        {
            logger.LogInformation("Processing request {RequestName}", requestName);

            var result = await nextHandler();

            logger.LogInformation("Completed request {RequestName}", requestName);

            return result;
        }
    }

    private static string GetModuleName(string requestName) => requestName.Split('.')[2];
}