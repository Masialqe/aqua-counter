using Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

public static class DatabaseServices
{
    public static IServiceCollection AddDatabaseServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContextFactory<ApplicationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        return services;
    }

    public static WebApplication ApplyDatabaseMigrations(this WebApplication app)
    {
        // Apply pending migrations at startup
        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            dbContext.Database.Migrate();
        }

        return app;
    }
}