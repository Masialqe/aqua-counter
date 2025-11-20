using Domain.RefreshTokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Configurations;

public sealed class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.HasKey(rt => rt.Id);
        builder.Property(rt => rt.Id).HasColumnName("RefreshTokenId");

        builder.Property(rt => rt.Token).HasColumnName("RefreshTokenValue").HasMaxLength(256);
        builder.Property(rt => rt.UserId).HasColumnName("RefreshTokenUser").HasMaxLength(100);

        builder.Property(rt => rt.ExpiresOnUtc).HasColumnName("RefreshTokenExpires");

        builder.HasIndex(rt => rt.Token).IsUnique();
        builder.HasIndex(rt => rt.UserId);
    }
}