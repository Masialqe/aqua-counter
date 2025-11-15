using Microsoft.AspNetCore.Identity;

namespace Identity;

public class ApplicationUser : IdentityUser
{
   public static ApplicationUser Create(string email)
   {
       return new ApplicationUser
       {
           UserName = email,
           Email = email
       };
   }
}