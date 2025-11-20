using Common.Abstractions;
using Identity;

namespace Domain.RefreshTokens;

public sealed class RefreshToken
{
    public RefreshToken() { }
    private RefreshToken(string token, string userId, DateTime expireOnUtc)
    {
        Token = token;
        UserId = userId;
        ExpiresOnUtc = expireOnUtc;
    }

    public static RefreshToken Create(string token, string userId, DateTime expireOnUtc)
        => new(token, userId, expireOnUtc);

    public Guid Id { get; set; } = Guid.CreateVersion7();
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresOnUtc { get; set; }

    public string? UserId { get; set; }

    public Guid UserDeviceId { get; set; }
    public UserDevice? UserDevice { get; set; }
}

public static class RefreshTokenOptions
{
    public static string TokenName = "refresh_token";
    public static string DeviceId = "device_id";
    public static int DaysToExpire = 14;
}

public static class RefreshTokenErrors
{
    public static Error RefreshTokenNotValidError => new(nameof(RefreshTokenNotValidError), "Refresh token is not valid.");
    public static Error RefreshTokenNotFoundError => new(nameof(RefreshTokenNotFoundError), "Refresh hasnt been found.");
}