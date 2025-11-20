using Common.Abstractions;

namespace Domain.Users;

public static class UserErrors
{
    public static Error UserNotCreatedError => new Error(nameof(UserNotCreatedError), "The user could not be created.");
    public static Error UserAlreadyExistsError => new Error(nameof(UserAlreadyExistsError), "Given email address is occupied.");
    public static Error UserNotConfirmedError => new Error(nameof(UserNotConfirmedError), "Cannot confirm user account.");
    public static Error UserNotAuthenticatedError = new Error(nameof(UserNotAuthenticatedError), "User validation failed.");

    public static Error User2faCodeInvalidError = new(nameof(User2faCodeInvalidError), "Provided 2fa code is invalid.");
}