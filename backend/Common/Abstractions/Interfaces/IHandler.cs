namespace Common.Abstractions.Interfaces;

public interface IHandler<TRequest, TResponse>
{
    Task<Result<TResponse>> HandleAsync(TRequest request, CancellationToken cancellationToken);
}