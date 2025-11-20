using Common.Extensions;
using Domain.RefreshTokens;
using Identity;

namespace Services;

public sealed class AuthenticationService(
    UserDeviceService userDeviceService,
    AccessTokenService tokenService,
    RefreshTokenService refreshTokenService,
    ILogger<AuthenticationService> logger)
{
    public async Task<string> LoginUserAsync(HttpContext httpContext, ApplicationUser user,
    CancellationToken cancellationToken = default)
    {
        var userAgentData = httpContext.GetUserAgentData();
        var deviceId = await userDeviceService.GetOrCreateDeviceAsync(user.Id, userAgentData);

        var accessToken = await tokenService.GenerateAccessTokenAsync(user);
        var refreshToken = await refreshTokenService.GenerateRefreshToken(user.Id, deviceId, RefreshTokenOptions.DaysToExpire);

        httpContext.AppendRefreshToken(refreshToken);
        httpContext.AppendDeviceId(deviceId);

        logger.LogInformation("User {UserId} - {UserEmail} has been logged in.", user.Id, user.Email);
        return accessToken;
    }
}