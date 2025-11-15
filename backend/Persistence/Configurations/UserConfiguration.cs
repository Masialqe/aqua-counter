using Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Configurations;


public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id).HasColumnName("UserId");

        builder.Property(u => u.Name)
            .HasColumnName("UserName")
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.Surname)
            .HasColumnName("UserSurname")
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.Email)
            .HasColumnName("UserEmail")
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.IdentityId)
            .HasColumnName("UserIdentityId")
            .HasMaxLength(100)
            .IsRequired();

        builder.HasOne(u => u.Address)
            .WithMany(a => a.Users)
            .OnDelete(DeleteBehavior.SetNull);
    }
}