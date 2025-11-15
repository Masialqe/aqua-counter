using Identity.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Common.Extensions
{
    public static class InitialSeederExtension
    {
        public static async Task SeedBaseRolesAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var adminRole = await roleManager.FindByNameAsync(Roles.Admin);

            if (adminRole == null)
            {
                await roleManager.CreateAsync(adminRole = new IdentityRole(Roles.Admin));
                await roleManager.AddClaimAsync(adminRole, new System.Security.Claims.Claim(ClaimTypes.Permission, Permissions.UserManage));
                await roleManager.AddClaimAsync(adminRole, new System.Security.Claims.Claim(ClaimTypes.Permission, Permissions.UserRead));
            }

            var baseUserRole = await roleManager.FindByNameAsync(Roles.User);

            if (baseUserRole == null)
            {
                await roleManager.CreateAsync(baseUserRole = new IdentityRole(Roles.User));
                await roleManager.AddClaimAsync(baseUserRole, new System.Security.Claims.Claim(ClaimTypes.Permission, Permissions.UserRead));
            }
        }
    }
}