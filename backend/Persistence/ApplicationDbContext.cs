using Domain.Addresses;
using Domain.Groups;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Persistence.Configurations;

namespace Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Group> Groups { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
       modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly, ConfigurationFilter);
       base.OnModelCreating(modelBuilder);
    }

     private static bool ConfigurationFilter(Type type) =>
        type.FullName?.Contains("Persistence.Configuration") ?? false;
}