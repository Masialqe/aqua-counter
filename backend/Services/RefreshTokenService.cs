using System.Security.Cryptography;
using Common.Abstractions;
using Domain.RefreshTokens;
using Identity;
using Microsoft.EntityFrameworkCore;

namespace Services;

public sealed class RefreshTokenService(IdentityContext context)
{

    public async Task<string> GenerateRefreshToken(string userId, string deviceId, int daysToExpire = 14)
    {

        if (!Guid.TryParse(deviceId, out var deviceGuid))
            throw new ArgumentException("Invalid deviceId", nameof(deviceId));

        // Get current device
        var currentDevice = await context.UserDevices
            .Where(ud => ud.Id == Guid.Parse(deviceId))
            .Include(ud => ud.RefreshToken)
            .FirstOrDefaultAsync();

        if (currentDevice is null) throw new InvalidOperationException("Cannot create access token for non existing device.");

        // Create new token value
        var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));

        // If device has no token
        if (currentDevice.RefreshToken is null)
        {
            var refreshToken = RefreshToken.Create(token, userId, DateTime.UtcNow.AddDays(daysToExpire));
            await context.RefreshTokens.AddAsync(refreshToken);
            currentDevice.RefreshToken = refreshToken;
        }
        // If device has old token
        else
        {
            currentDevice.RefreshToken!.Token = token;
            currentDevice.RefreshToken!.ExpiresOnUtc = DateTime.UtcNow.AddDays(daysToExpire);
        }

        await context.SaveChangesAsync();
        return token;
    }

    public async Task<Result<RefreshToken>> IsRefreshTokenValid(string refreshToken, string deviceId)
    {
        var tokenState = await context.RefreshTokens
            .AsNoTracking()
            .FirstOrDefaultAsync(rf => rf.Token == refreshToken);

        if (tokenState is null) return RefreshTokenErrors.RefreshTokenNotFoundError;

        if (tokenState.ExpiresOnUtc < DateTime.UtcNow) return RefreshTokenErrors.RefreshTokenNotValidError;

        if (!Guid.TryParse(deviceId, out var deviceGuid) || tokenState.UserDeviceId != deviceGuid)
            return RefreshTokenErrors.RefreshTokenNotValidError;

        return tokenState;
    }

}