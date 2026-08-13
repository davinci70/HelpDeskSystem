using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace HelpDesk.API.Exceptions;

public sealed class GlobalExceptionHandler
    : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(
        ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(
            exception,
            "Unhandled exception. TraceId: {TraceId}",
            httpContext.TraceIdentifier);

        var (statusCode, title, detail) = exception switch
        {
            ValidationException validationException =>
                (
                    StatusCodes.Status400BadRequest,
                    "Validation Error",
                    validationException.Message
                ),

            UnauthorizedAccessException =>
                (
                    StatusCodes.Status401Unauthorized,
                    "Unauthorized",
                    exception.Message
                ),

            KeyNotFoundException =>
                (
                    StatusCodes.Status404NotFound,
                    "Resource Not Found",
                    exception.Message
                ),

            InvalidOperationException =>
                (
                    StatusCodes.Status409Conflict,
                    "Conflict",
                    exception.Message
                ),

            _ =>
                (
                    StatusCodes.Status500InternalServerError,
                    "Internal Server Error",
                    "An unexpected error occurred."
                )
        };

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = detail,
            Instance = httpContext.Request.Path
        };

        problemDetails.Extensions["traceId"] = httpContext.TraceIdentifier;

        httpContext.Response.StatusCode = statusCode;
        httpContext.Response.ContentType = "application/problem+json";

        await httpContext.Response.WriteAsJsonAsync(
            problemDetails,
            cancellationToken);

        return true;
    }
}