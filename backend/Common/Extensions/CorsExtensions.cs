namespace Common.Extensions;

public static class CorsExtensions
{
    private const string developmentPolicyName = "DevelopmentCorsPolicy";
    public static IServiceCollection AddDevelopmentsCorsSettings(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(developmentPolicyName, builder =>
            {
                builder
                    .AllowAnyOrigin()
                    .AllowCredentials()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });

        return services;
    }
}