using System.Security.Claims;
using Common.Abstractions.Interfaces;
using Common.Responses;
using Domain.Users;
using Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Features.Auth;

public static class Enable2fa
{
    public sealed record Enable2faRequest(string Code);
    public sealed record Enable2faResponse(IEnumerable<string> RecoveryCodes);

    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("/auth/2fa/enable", async (Enable2faRequest request,
            [FromServices]Enable2faHandler handler,
            ClaimsPrincipal claimsPrincipal,
            CancellationToken cancellationToken)
                => await handler.HandleAsync(request, claimsPrincipal, cancellationToken))
           .RequireAuthorization();
        }
    }

    public sealed class Enable2faHandler(
        UserManager<ApplicationUser> userManager,
        ILogger<Endpoint> logger) : IHandler
    {
        public async Task<IResult> HandleAsync(Enable2faRequest request,
        ClaimsPrincipal principal,
        CancellationToken cancellationToken = default)
        {
            var user = await userManager.GetUserAsync(principal);
            if (user is null) return ApiResult.Unauthorized(UserErrors.UserNotAuthenticatedError);

            var isCodeValid = await userManager.VerifyTwoFactorTokenAsync(
               user,
               TokenOptions.DefaultPhoneProvider,
               request.Code);

            if (!isCodeValid) return ApiResult.BadRequest(UserErrors.User2faCodeInvalidError);

            await userManager.SetTwoFactorEnabledAsync(user, true);

            var recoveryCodes = await userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 5);

            logger.LogInformation("User {UserId} has enabled two-factor authentication.", user.Id);
            return ApiResult.Ok(new Enable2faResponse(recoveryCodes!));
        }
    }
}