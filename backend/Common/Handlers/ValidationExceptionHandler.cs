using Common.Responses;
using Domain.Errors;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;

namespace Common.Handlers;

internal sealed class ValidationExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not ValidationException validationException)
        {
            return false;
        }

        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        httpContext.Response.ContentType = "application/json";

        var errors = validationException.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(e => e.ErrorMessage).ToArray()
            );

        string errorsString = "{" + string.Join(", ",
            errors.Select(kv => $"{kv.Key}: [{string.Join(", ", kv.Value.Select(v => $"\"{v}\""))}]")) + "}";

        var response = ApiResponse.Fail(
            GeneralErrors.ValidationError(errorsString),
            StatusCodes.Status400BadRequest
        );

        await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);
        return true;
    }
}