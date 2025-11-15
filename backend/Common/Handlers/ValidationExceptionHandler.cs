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

        var errorList = errors.SelectMany(kv => kv.Value).ToList();

        var response = ApiResponse.Fail("One or more validation errors occurred.",
            errorList,
            StatusCodes.Status400BadRequest);
            
        await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);
        return true;
    }
}