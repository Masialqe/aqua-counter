using Domain.RefreshTokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Configurations;

public sealed class UserDeviceConfiguration : IEntityTypeConfiguration<UserDevice>
{
    public void Configure(EntityTypeBuilder<UserDevice> builder)
    {
        builder.HasKey(ud => ud.Id);
        builder.Property(ud => ud.Id).HasColumnName("DeviceId");

        builder.Property(ud => ud.UserAgent).HasColumnName("DeviceAgent").HasMaxLength(255);
        builder.Property(ud => ud.IpAddress).HasColumnName("DeviceIpAddress").HasMaxLength(255);
        builder.Property(ud => ud.LastUsedUtc).HasColumnName("DeviceLastUsedUtc");
        builder.Property(ud => ud.CreatedAtUrc).HasColumnName("DeviceCreatedAt");

        builder.HasOne(ud => ud.RefreshToken)
       .WithOne(rt => rt.UserDevice)
       .HasForeignKey<RefreshToken>(rt => rt.UserDeviceId)
       .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(ud => new { ud.UserId, ud.IpAddress, ud.UserAgent })
        .IsUnique();

        builder.HasIndex(ud => ud.IpAddress);
        builder.HasIndex(ud => ud.UserAgent);
    }
}