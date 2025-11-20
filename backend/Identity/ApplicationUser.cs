using Microsoft.AspNetCore.Identity;

namespace Identity;

public class ApplicationUser : IdentityUser
{
    public ICollection<UserDevice> UserDevices { get; set; } = [];

    public static ApplicationUser Create(string email)
    {
        return new ApplicationUser
        {
            UserName = email,
            Email = email
        };
    }
}