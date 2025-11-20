using Common.Abstractions.Interfaces;
using Common.Extensions;
using Common.Responses;
using Domain.RefreshTokens;
using Domain.Users;
using Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace Features.Auth;

public sealed class RefreshUser
{
    public sealed record RefreshUserResponse(string AccessToken);

    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("/auth/refresh", async (HttpContext httpContext, [FromServices]RefreshUserHandler handler,
            CancellationToken cancellationToken) => handler.HandleAsync(httpContext, cancellationToken));
        }
    }

    public sealed class RefreshUserHandler(
        RefreshTokenService refreshTokenService,
        AccessTokenService accessTokenService,
        UserManager<ApplicationUser> userManager,
        ILogger<Endpoint> logger) : IHandler
    {
        public async Task<IResult> HandleAsync(HttpContext httpContext,
        CancellationToken cancellationToken = default)
        {
            var token = httpContext.GetRefreshToken();
            var deviceId = httpContext.GetDeviceId();

            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(deviceId)) return ApiResult.Unauthorized(UserErrors.UserNotAuthenticatedError);

            var tokenValidationResult = await refreshTokenService.IsRefreshTokenValid(token, deviceId);

            if (tokenValidationResult.IsFailure) return ApiResult.Unauthorized(tokenValidationResult.Error);

            var user = await userManager.FindByIdAsync(tokenValidationResult.Value.UserId!);

            if (user is null) return ApiResult.Unauthorized(tokenValidationResult.Error);

            var accessToken = await accessTokenService.GenerateAccessTokenAsync(user);
            var refreshToken = await refreshTokenService.GenerateRefreshToken(user.Id, deviceId, RefreshTokenOptions.DaysToExpire);

            httpContext.AppendRefreshToken(refreshToken);

            logger.LogInformation("User {UserId} has logged using refresh token.", user.Id);
            return ApiResult.Ok(new RefreshUserResponse(accessToken));
        }
    }
}
