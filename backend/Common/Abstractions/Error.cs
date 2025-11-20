using Common.Responses;

namespace Common.Abstractions;

public record Error(
    string errorName,
    string? errorDescription = null)
{
    public static readonly Error None = new(string.Empty);

    public static implicit operator Result(Error error)
        => Result.Failure(error);

    public static implicit operator Error(Exception exception)
        => new(nameof(exception), exception.ToString());

    public static implicit operator List<string>(Error error)
        => error.ToErrorList();

}