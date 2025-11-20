namespace Common.Abstractions.Interfaces;

public interface IHandler { }

public interface IHandler<TRequest, TResponse> : IHandler
{
    Task<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken);
}