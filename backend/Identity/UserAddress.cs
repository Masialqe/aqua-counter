namespace Identity;

public sealed class UserAddress
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public string Street { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public string BuildingNumber { get; set; } = string.Empty;
    public string ApartmentNumber { get; set; } = string.Empty;

    // Foreign key to ApplicationUser
    public string ApplicationUserId { get; set; } = string.Empty;
    public ApplicationUser ApplicationUser { get; set; } = null!;
}