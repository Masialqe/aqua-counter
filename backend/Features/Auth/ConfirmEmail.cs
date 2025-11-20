using System.Net;
using Common.Abstractions;
using Common.Abstractions.Interfaces;
using Common.Responses;
using Domain.Users;
using Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Features.Auth;


public static class ConfirmEmail
{
    public record ConfirmEmailRequest(string UserId, string Token);

    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("/auth/confirm-email", async (
                string userId,
                string token,
                [FromServices]ConfirmEmailHandler handler,
                CancellationToken cancellationToken)
                => await handler.HandleAsync(new ConfirmEmailRequest(userId, token), cancellationToken));
        }
    }

    public sealed class ConfirmEmailHandler(UserManager<ApplicationUser> userManager,
        ILogger<Endpoint> logger) : IHandler
    {
        public async Task<IResult> HandleAsync(ConfirmEmailRequest request,
        CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(request.UserId) || string.IsNullOrEmpty(request.Token))
                return ApiResult.BadRequest(UserErrors.UserNotConfirmedError);

            var user = await userManager.FindByIdAsync(request.UserId);
            if (user is null) return ApiResult.BadRequest(UserErrors.UserNotConfirmedError);

            var decodedToken = WebUtility.UrlDecode(request.Token);

            var result = await userManager.ConfirmEmailAsync(user, decodedToken);

            if (!result.Succeeded)
            {
                var resultErrors = string.Join(", ", result.Errors.Select(e => e.Description));
                logger.LogInformation("Failed to confirm user {UserId} due to {Error}", user.Id, resultErrors);
                return ApiResult.BadRequest(UserErrors.UserNotConfirmedError);
            }

            return ApiResult.NoContent();
        }
    }
}