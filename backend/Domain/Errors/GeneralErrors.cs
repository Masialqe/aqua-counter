using Common.Abstractions;

namespace Domain.Errors;

public static class GeneralErrors
{
    public static Error ApplicationError => new Error(
        errorName: "ApplicationError",
        errorDescription: "An unexpected error occurred in the application.");

    public static Error ValidationError(string details) => new Error(
        errorName: "ValidationError",
        errorDescription: $"One or more validation errors occurred. Details: {details}");
}