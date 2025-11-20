using Domain.RefreshTokens;

namespace Identity;

public class UserDevice
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public string UserId { get; set; } = string.Empty;
    public ApplicationUser? User { get; set; }
    public string IpAddress { get; set; } = string.Empty;
    public string UserAgent { get; set; } = string.Empty;
    public DateTime LastUsedUtc { get; set; } = DateTime.UtcNow;
    public DateTime CreatedAtUrc { get; set; } = DateTime.UtcNow;

    public RefreshToken? RefreshToken { get; set; } = default!;

    public UserDevice() { }
    private UserDevice(string userId, string ipAddress, string userAgent)
    {
        UserId = userId;
        IpAddress = ipAddress;
        UserAgent = userAgent;
    }

    public static UserDevice Create(string userId, string ipAddress, string userAgent)
        => new(userId, ipAddress, userAgent);
}