using backend.Services;
using Services;

namespace Common.Extensions;

public static class RegisterServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<AuthenticationService>();
        services.AddScoped<EmailNotificationService>();
        services.AddScoped<RefreshTokenService>();
        services.AddScoped<AccessTokenService>();
        services.AddScoped<UserDeviceService>();

        return services;
    }
}