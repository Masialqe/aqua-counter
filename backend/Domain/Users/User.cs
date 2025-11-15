using Domain.Addresses;
using Domain.Groups;
using Identity;

namespace Domain.Users;

public sealed class User
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Guid IdentityId { get; set; }

    public Address? Address { get; set; }
    public Guid? AddressId { get; set; }

    public ICollection<Group> Groups { get; set; } = [];
}