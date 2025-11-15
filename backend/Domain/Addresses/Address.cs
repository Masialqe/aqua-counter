using Domain.Users;

namespace Domain.Addresses;

public sealed class Address
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public string Street { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public string BuildingNumber { get; set; } = string.Empty;
    public string ApartmentNumber { get; set; } = string.Empty;

    public ICollection<User> Users { get; set; } = [];
}