using Common.Abstractions.Interfaces;
using Common.Responses;
using Domain.Users;
using Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace Features.Auth;

public static class LoginWith2fa
{
    public sealed record LoginWith2faRequest(string UserId, string Code);
    public sealed record LoginWith2faResponse(string AccessToken, string UserId);
    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("/auth/login/2fa", async (LoginWith2faRequest request, [FromServices] LoginWith2faHandler handler,
            HttpContext httpContext,
            CancellationToken cancellationToken) => await handler.HandleAsync(request, httpContext, cancellationToken));
        }
    }

    public sealed class LoginWith2faHandler(
        UserManager<ApplicationUser> userManager,
        AuthenticationService authenticationService) : IHandler
    {
        public async Task<IResult> HandleAsync(LoginWith2faRequest request, HttpContext httpContext,
       CancellationToken cancellationToken = default)
        {
            var user = await userManager.FindByIdAsync(request.UserId);
            if (user is null) return ApiResult.Unauthorized(UserErrors.UserNotAuthenticatedError);

            // Validate 2fa code
            var isCodeValid = await userManager.VerifyTwoFactorTokenAsync(
                user,
                TokenOptions.DefaultPhoneProvider,
                request.Code);

            if (!isCodeValid) return ApiResult.Unauthorized(UserErrors.UserNotAuthenticatedError);

            string accessToken = await authenticationService.LoginUserAsync(httpContext, user);

            return ApiResult.Ok(new LoginWith2faResponse(accessToken, user.Id));
        }
    }
}

