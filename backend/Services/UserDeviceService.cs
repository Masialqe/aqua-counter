using Common.Extensions;
using Identity;
using Microsoft.EntityFrameworkCore;

namespace Services;

public sealed class UserDeviceService(IdentityContext identityContext)
{
    public async Task<string> GetOrCreateDeviceAsync(string userId, UserAgentData userAgentData,
    CancellationToken cancellationToken = default)
    {
        // Validate if device is existing
        if (!string.IsNullOrWhiteSpace(userAgentData.DeviceId)
        && Guid.TryParse(userAgentData.DeviceId, out var deviceGuid))
        {
            var exists = await identityContext.UserDevices
                .AnyAsync(ud => ud.Id == deviceGuid && ud.UserId == userId, cancellationToken);

            if (exists)
                return userAgentData.DeviceId!;
        }

        // Checking if this device has been already used by this user
        var similarDevice = await identityContext.UserDevices
        .Where(ud =>
            ud.UserId == userId &&
            ud.UserAgent == userAgentData.UserAgent &&
            ud.IpAddress == userAgentData.Ip)
        .FirstOrDefaultAsync(cancellationToken);

        //TODO: Refactor - not call SaveChangesAsync twice
        if (similarDevice is not null)
        {
            similarDevice.LastUsedUtc = DateTime.UtcNow;
            await identityContext.SaveChangesAsync();
            return similarDevice.Id.ToString();
        }

        // Create new device for that user
        var newDevice = UserDevice.Create(userId, userAgentData.Ip, userAgentData.UserAgent);
        await identityContext.AddAsync(newDevice, cancellationToken);
        await identityContext.SaveChangesAsync(cancellationToken);

        return newDevice.Id.ToString();
    }

    public async Task LoginDevice(string deviceId, CancellationToken cancellationToken = default)
        => await identityContext.UserDevices
            .Where(ud => ud.Id == Guid.Parse(deviceId))
            .ExecuteDeleteAsync(cancellationToken);

    public async Task LogoutDevice(string deviceId, CancellationToken cancellationToken = default)
        => await identityContext.UserDevices
            .Where(ud => ud.Id == Guid.Parse(deviceId))
            .ExecuteDeleteAsync(cancellationToken);
}