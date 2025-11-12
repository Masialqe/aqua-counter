using Microsoft.EntityFrameworkCore;

namespace Identity;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.Property(u => u.Name)
            .HasColumnName("UserGivenName")
            .HasMaxLength(100);

        builder.Property(u => u.Surname)
            .HasColumnName("UserSurname")
            .HasMaxLength(100);

        builder.HasOne(u => u.Address)
            .WithMany()
            .HasForeignKey(u => u.AddressId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasIndex(u => u.Name);
        builder.HasIndex(u => u.Surname);
    }
}

public class UserAddressConfiguration : IEntityTypeConfiguration<UserAddress>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<UserAddress> builder)
    {
        builder.Property(ua => ua.Street)
            .HasColumnName("AddressStreet")
            .HasMaxLength(200);

        builder.Property(ua => ua.City)
            .HasColumnName("AddressCity")
            .HasMaxLength(100);

        builder.Property(ua => ua.BuildingNumber)
            .HasColumnName("AddressBuildingNumber")
            .HasMaxLength(10);

        builder.Property(ua => ua.ApartmentNumber)
            .HasColumnName("AddressApartmentNumber")
            .HasMaxLength(10);

        builder.Property(ua => ua.PostalCode)
            .HasColumnName("AddressPostalCode")
            .HasMaxLength(20);
    }
}