namespace Common.Abstractions.Interfaces;

public interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder app);
}