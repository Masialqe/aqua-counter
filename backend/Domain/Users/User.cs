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

    public string IdentityId { get; set; } = string.Empty;

    public Address? Address { get; set; }
    public Guid? AddressId { get; set; }

    public ICollection<Group> Groups { get; set; } = [];

    public User() { }
    private User(string email, string name, string surname, string identityId)
    {
        Name = name;
        Email = email;
        Surname = surname;
        IdentityId = identityId;
    }

    public static User Create(string email, string name, string surname, string identityId)
        => new(email, name, surname, identityId);
}