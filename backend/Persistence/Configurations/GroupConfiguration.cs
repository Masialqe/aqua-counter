using Domain.Groups;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Configurations;

public class GroupConfiguration : IEntityTypeConfiguration<Group>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Group> builder)
    {
        builder.HasKey(g => g.Id);
        builder.Property(g => g.Id).HasColumnName("GroupId");

        builder.Property(g => g.Name)
            .HasColumnName("GroupName")
            .IsRequired()
            .HasMaxLength(100);
        
        builder.HasMany(g => g.Users)
        .WithMany(u => u.Groups)
        .UsingEntity(o => o.ToTable("UserGroups"));
    }
}