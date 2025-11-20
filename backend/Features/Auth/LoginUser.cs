using Common.Abstractions.Interfaces;
using Common.Responses;
using Domain.Users;
using Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace Features.Users;

public static class LoginUser
{
    public sealed record LoginUserRequest(string Email, string Password, bool RememberMe = false);
    public sealed record LoginUserResponse(string? accessToken, string UserId, bool RequiresTwoFactor)
    {
        public static LoginUserResponse CreateWithToken(string? accessToken, string userId)
            => new(accessToken, userId, false);

        public static LoginUserResponse CreateWith2fa(string userId)
            => new(null, userId, true);
    }
    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("/auth/login", async (LoginUserRequest request, [FromServices]LoginUserHandler handler,
            HttpContext httpContext,
            CancellationToken cancellationToken) => await handler.HandleAsync(request, httpContext, cancellationToken));
        }
    }

    public sealed class LoginUserHandler(UserManager<ApplicationUser> userManager,
        AuthenticationService authenticationService) : IHandler
    {
        public async Task<IResult> HandleAsync(LoginUserRequest request,
        HttpContext httpContext,
        CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
                return ApiResult.Unauthorized(UserErrors.UserNotAuthenticatedError);

            // Validate user
            var user = await userManager.FindByEmailAsync(request.Email);
            if (user is null) return ApiResult.Unauthorized(UserErrors.UserNotAuthenticatedError);

            // Validate password
            var isPasswordValid = await userManager.CheckPasswordAsync(user, request.Password);
            if (!isPasswordValid) return ApiResult.Unauthorized(UserErrors.UserNotAuthenticatedError);

            // Validate block
            if (await userManager.IsLockedOutAsync(user)) return ApiResult.Unauthorized(UserErrors.UserNotAuthenticatedError);

            // Validate 2fa
            var requiresTwoFactor = await userManager.GetTwoFactorEnabledAsync(user);
            if (requiresTwoFactor) return ApiResult.Ok(LoginUserResponse.CreateWith2fa(user.Id));

            string accessToken = await authenticationService.LoginUserAsync(httpContext, user);

            return ApiResult.Ok(LoginUserResponse.CreateWithToken(accessToken, user.Id));
        }
    }
}