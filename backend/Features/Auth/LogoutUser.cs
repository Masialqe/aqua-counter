using Common.Abstractions.Interfaces;
using Common.Errors;
using Common.Extensions;
using Common.Responses;
using Domain.RefreshTokens;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace Features.Auth;

public static class LogoutUser
{
    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("/auth/logout", async (HttpContext httpContext, [FromServices] LogoutUserHandler handler,
            CancellationToken cancellationToken = default)
                => await handler.HandleAsync(httpContext, cancellationToken))
                .RequireAuthorization();
        }
    }

    public sealed class LogoutUserHandler(
        UserDeviceService userDeviceService,
        ILogger<Endpoint> logger) : IHandler
    {
        public async Task<IResult> HandleAsync(HttpContext httpContext,
            CancellationToken cancellationToken = default)
        {
            httpContext.Response.Cookies.Delete(RefreshTokenOptions.TokenName);

            var deviceId = httpContext.GetDeviceId();
            if (deviceId is null) return ApiResult.BadRequest(CommonErrors.ApplicationError("Cannot logout user."));

            httpContext.Response.Cookies.Delete(RefreshTokenOptions.DeviceId);

            await userDeviceService.LogoutDevice(deviceId, cancellationToken);

            logger.LogInformation("Device {DeviceId} has been logged out.", deviceId);
            return ApiResult.Ok("User has been logged out.");
        }
    }
}