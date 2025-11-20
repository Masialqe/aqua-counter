using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Identity.Authorization;

namespace Services;

public sealed class AccessTokenService(
    IConfiguration configuration,
    IdentityContext identityContext,
    UserManager<ApplicationUser> userManager)
{
    public async Task<string> GenerateAccessTokenAsync(ApplicationUser user)
    {
        var jwtOptions = GetJwtOptions();
        var key = Encoding.UTF8.GetBytes(jwtOptions.Key);

        var roles = await userManager.GetRolesAsync(user);
        var permissions = await GetPermissionsAsync(roles);

        List<Claim> claims =
        [
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            ..roles.Select(r => new Claim(ClaimTypes.Role, r)),
            ..permissions.Select(p => new Claim(ApplicationClaimTypes.Permission, p!))
        ];

        var tokenDescripton = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(jwtOptions.ExpiresInMinutes),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha512Signature
            ),
            Issuer = jwtOptions.Issuer,
            Audience = jwtOptions.Audience
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescripton);

        return tokenHandler.WriteToken(token);
    }

    private async Task<List<string?>> GetPermissionsAsync(IList<string> roles)
    {
        return await identityContext.Roles
            .Where(role => roles.Contains(role.Name!))
            .Join(
                identityContext.RoleClaims,
                role => role.Id,
                claim => claim.RoleId,
                (role, claim) => claim
            )
            .Where(claim => claim.ClaimType == ApplicationClaimTypes.Permission)
            .Select(claim => claim.ClaimValue)
            .Distinct()
            .ToListAsync();
    }

    public JwtOptions GetJwtOptions()
    {
        var key = configuration["Jwt:Key"]
            ?? throw new InvalidOperationException("Key must be configured in appsetings.");

        var audience = configuration["Jwt:Audience"]
            ?? throw new InvalidOperationException("Audience must be configured in appsetings.");

        var issuer = configuration["Jwt:Issuer"]
            ?? throw new InvalidOperationException("Issuer must be configured in appsetings.");

        if (!int.TryParse(configuration["Jwt:ExpireMinutes"], out int expiresInMinutes))
            throw new InvalidOperationException("Invalid value in Jwt.ExpireMinutes");

        return new JwtOptions(key, audience, issuer, expiresInMinutes);
    }
}

public sealed record JwtOptions(string Key, string Audience, string Issuer, int ExpiresInMinutes);
