using System.Net;
using backend.Services;
using Common.Abstractions.Interfaces;
using Common.Responses;
using Domain.Dictionaries;
using Domain.Users;
using Identity;
using Identity.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Persistence;

namespace Features.Users;

public static class RegisterUser
{
    public sealed record RegisterUserRequest(string Email, string Password, string Name, string Surname);
    public sealed record RegisterUserResponse(string Email)
    {
        public static RegisterUserResponse Create(string email) => new(email);
    };

    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("/auth/register", async (RegisterUserRequest request, [FromServices] RegisterUserHandler handler,
            CancellationToken cancellationToken) => await handler.HandleAsync(request, cancellationToken));
        }
    }

    public sealed class RegisterUserHandler(UserManager<ApplicationUser> identityManager,
        EmailNotificationService emailNotificationService,
        IdentityContext identityContext,
        IDbContextFactory<ApplicationDbContext> applicationContextFactory) : IHandler
    {
        public async Task<IResult> HandleAsync(RegisterUserRequest request, CancellationToken cancellationToken)
        {

            var isEmailTaken = await identityContext.Users.AnyAsync(u => u.Email == request.Email, cancellationToken);
            if (isEmailTaken) return ApiResult.BadRequest(UserErrors.UserAlreadyExistsError);

            // Prepare transaction
            await using var transaction = await identityContext.Database.BeginTransactionAsync(cancellationToken);

            await using var applicationDbContext = await applicationContextFactory.CreateDbContextAsync();

            applicationDbContext.Database.SetDbConnection(identityContext.Database.GetDbConnection());
            await applicationDbContext.Database.UseTransactionAsync(transaction.GetDbTransaction());

            var identityUser = ApplicationUser.Create(request.Email);

            try
            {
                // Create identity 
                var createIdentityUserResult = await identityManager.CreateAsync(identityUser, request.Password);
                if (!createIdentityUserResult.Succeeded) return ApiResult.BadRequest(UserErrors.UserNotCreatedError);

                var addToRoleResult = await identityManager.AddToRoleAsync(identityUser, Roles.User);
                if (!addToRoleResult.Succeeded) return ApiResult.BadRequest(UserErrors.UserNotCreatedError);

                //Domain user
                var domainUser = User.Create(request.Email, request.Name, request.Surname, identityUser.Id);
                await applicationDbContext.Users.AddAsync(domainUser, cancellationToken);

                await applicationDbContext.SaveChangesAsync(cancellationToken);

                await transaction.CommitAsync(cancellationToken);
            }
            catch
            {
                await transaction.RollbackAsync();
                return ApiResult.BadRequest(UserErrors.UserNotCreatedError, "Cannot register due to internal error.");
            }

            //Send Confirmation Email
            var emailConfirmationToken = await identityManager.GenerateEmailConfirmationTokenAsync(identityUser);
            var encodedToken = WebUtility.UrlEncode(emailConfirmationToken);

            await emailNotificationService.SendMessageAsync(
                request.Email,
                NotificationTitle.ActivateAccountToken,
                NotificationContent.ActivateAccountMessage(encodedToken, identityUser.Id));

            return ApiResult.Created("/users/me", RegisterUserResponse.Create(request.Email));
        }
    }
}