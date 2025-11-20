using System.Net.Http.Headers;
using Domain.RefreshTokens;
using Identity.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Identity;

public class IdentityContext : IdentityDbContext<ApplicationUser>
{
    public IdentityContext(DbContextOptions<IdentityContext> options) : base(options) { }

    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<UserDevice> UserDevices { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfiguration(new ApplicationUserConfiguration());
        builder.ApplyConfiguration(new RefreshTokenConfiguration());
        builder.ApplyConfiguration(new UserDeviceConfiguration());
    }

}