using Common.Abstractions;

namespace Common.Errors;

public static class CommonErrors
{
    public static Error ApplicationError(string description) => new(nameof(ApplicationError), description);
}