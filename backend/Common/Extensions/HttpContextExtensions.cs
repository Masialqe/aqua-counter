using Domain.RefreshTokens;

namespace Common.Extensions;

public static class HttpContextExtensions
{
    public static string? GetDeviceId(this HttpContext httpContext)
        => httpContext.Request.Cookies[RefreshTokenOptions.DeviceId];

    public static string? GetRefreshToken(this HttpContext httpContext)
        => httpContext.Request.Cookies[RefreshTokenOptions.TokenName];

    public static void AppendRefreshToken(this HttpContext httpContext, string refreshToken)
        => httpContext.Response.Cookies.Append(
            RefreshTokenOptions.TokenName,
            refreshToken,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(RefreshTokenOptions.DaysToExpire)
            }
        );

    public static void AppendDeviceId(this HttpContext httpContext, string deviceId)
        => httpContext.Response.Cookies.Append(
            RefreshTokenOptions.DeviceId,
            deviceId,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(RefreshTokenOptions.DaysToExpire)
            }
        );

    public static UserAgentData GetUserAgentData(this HttpContext httpContext)
        => new UserAgentData(
            httpContext.Request.Headers["User-Agent"].ToString(),
            httpContext.Connection.RemoteIpAddress?.ToString()!,
            httpContext.Request.Cookies[RefreshTokenOptions.DeviceId]
        );
}

public sealed record UserAgentData(string UserAgent, string Ip, string? DeviceId);
