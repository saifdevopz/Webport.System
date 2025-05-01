﻿using Common.Infrastructure.Authentication;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Common.Infrastructure.Middlewares;

public sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
    {
#pragma warning disable CA1848 // Use the LoggerMessage delegates
        logger.LogError(exception, "Global Exception Handler - Unhandled Exception Occurred");
#pragma warning restore CA1848 // Use the LoggerMessage delegates

        ProblemDetails problemDetails = new()
        {
            Status = StatusCodes.Status500InternalServerError,
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1",
            Title = "An unhandled exception occurred in the application",
            Detail = exception.Message
        };

        // Exception
        problemDetails.Extensions["Exception"] = new
        {
            Type = exception.GetType().FullName,
            StackTrace = exception.StackTrace!,
            Source = exception.Source!
        };

        // Inner Exception
        List<object>? innerExceptionDetails = GetInnerExceptionDetails(exception.InnerException);
        if (innerExceptionDetails is not null)
        {
            problemDetails.Extensions["InnerExceptions"] = innerExceptionDetails;
        }

        // Custom Exception
        if (exception is CustomException starterException)
        {
            problemDetails.Extensions["StarterException"] = new
            {
                starterException.RequestName
            };
        }

#pragma warning disable CA1848 // Use the LoggerMessage delegates
        logger.LogInformation("Problem details created: {@ProblemDetails}", problemDetails);
#pragma warning restore CA1848 // Use the LoggerMessage delegates
        // Set response properties
        httpContext.Response.StatusCode = problemDetails.Status.Value;
        httpContext.Response.ContentType = "application/problem+json";

        // Write problem details to the response
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }

    private static List<object>? GetInnerExceptionDetails(Exception? exception)
    {
        if (exception is null)
        {
            return null;
        }

        List<object> innerExceptions = [];
        while (exception is not null)
        {
            innerExceptions.Add(new
            {
                Type = exception.GetType().FullName,
                Message = exception.Message!,
                StackTrace = exception.StackTrace!,
                Source = exception.Source!
            });
            exception = exception.InnerException;
        }
        return innerExceptions;
    }
}