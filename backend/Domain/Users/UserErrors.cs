using Common.Abstractions;

namespace Domain.Users;

public static class UserErrors
{
    public static Error UserNotCreatedError => new Error(nameof(UserNotCreatedError), "The user could not be created.");
}