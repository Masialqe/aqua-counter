using System.Text.Json;
using Common.Responses;
using Domain.Errors;
using Microsoft.AspNetCore.Diagnostics;

namespace Common.Handlers;

internal sealed class GlobalExceptionHandler(
    ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        logger.LogError("Unhandled exception occurred: {ExceptionTitle}: {ExceptionMessage}", exception.Message, exception.StackTrace);

        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        httpContext.Response.ContentType = "application/json";

        var response = ApiResponse.Fail(
            GeneralErrors.ApplicationError,
            StatusCodes.Status500InternalServerError
        );

        await httpContext.Response.WriteAsync(JsonSerializer.Serialize(response), cancellationToken);
        return true;
    }
}