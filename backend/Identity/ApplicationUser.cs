using Microsoft.AspNetCore.Identity;

namespace Identity;

public class ApplicationUser : IdentityUser
{
    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;

    public UserAddress? Address { get; set; }
    public Guid? AddressId { get; set; }
}