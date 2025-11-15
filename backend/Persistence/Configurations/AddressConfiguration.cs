using Domain.Addresses;
using Identity;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Configurations;


public class AddressConfiguration : IEntityTypeConfiguration<Address>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Address> builder)
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