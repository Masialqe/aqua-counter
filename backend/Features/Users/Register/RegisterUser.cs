using System.Transactions;
using Common.Abstractions;
using Common.Abstractions.Interfaces;
using Common.Responses;
using Domain.Users;
using Identity;
using Identity.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

namespace Features.Users.Register;

public static class RegisterUser
{
    public sealed record RegisterUserRequest(string Email, string Password, string Name, string Surname);
    public sealed record RegisterUserResponse(string Email);

    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("/users/register", async ([FromBody] RegisterUserRequest request,
            CancellationToken cancellationToken) =>
            {
                // Implementation goes here

                return Results.Created();
            });
        }
    }

    public sealed class RegisterUserHandler(
        UserManager<ApplicationUser> identityManager) : IHandler<RegisterUserRequest, RegisterUserResponse>
    {

        public async Task<Result<RegisterUserResponse>> HandleAsync(RegisterUserRequest request, CancellationToken cancellationToken)
        {
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            //Create Identity User
            var user = ApplicationUser.Create(request.Email);
            
            var createIdentityUserResult = await identityManager.CreateAsync(user, request.Password);
            if (!createIdentityUserResult.Succeeded) return UserErrors.UserNotCreatedError;

            await identityManager.AddToRoleAsync(user, Roles.User);

            //Create Domain User

            //Send Confirmation Email
            var emailConfirmationToken = await identityManager.GenerateEmailConfirmationTokenAsync(user);

            return new RegisterUserResponse(request.Email);
        }
    }

}