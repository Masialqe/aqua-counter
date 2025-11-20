using System.Security.Claims;
using Common.Abstractions.Interfaces;
using Identity;
using Microsoft.AspNetCore.Identity;
using Common.Responses;
using Domain.Users;
using Microsoft.AspNetCore.Mvc;

namespace Features.Auth.TwoFactor;

public static class Setup2fa
{
    public sealed record Setup2faResponse(string TwoFactorKey, string Url);
    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("/auth/2fa/setup", async (ClaimsPrincipal claimsPrincipal, [FromServices]Setup2faHandler handler)
            => await handler.HandleAsync(claimsPrincipal))
            .RequireAuthorization();
        }
    }

    public sealed class Setup2faHandler(
        UserManager<ApplicationUser> userManager,
        IConfiguration configuration,
        ILogger<Endpoint> logger) : IHandler
    {
        public async Task<IResult> HandleAsync(ClaimsPrincipal principal)
        {
            var user = await userManager.GetUserAsync(principal);
            if (user is null) return ApiResult.Unauthorized(UserErrors.UserNotAuthenticatedError);

            var key = await userManager.GetAuthenticatorKeyAsync(user);

            if (string.IsNullOrEmpty(key))
            {
                await userManager.ResetAuthenticatorKeyAsync(user);
                key = await userManager.GetAuthenticatorKeyAsync(user);
            }

            var issuer = configuration["Jwt:Issuer"];
            var email = user.Email;
            var twoFactorUrl = $"otpauth://totp/{issuer}:{email}?secret={key}&issuer={issuer}&digits=6";

            logger.LogInformation("User {UserId} has set up two-factor authentication.", user.Id);
            return ApiResult.Ok(new Setup2faResponse(key!, twoFactorUrl));
        }
    }
}