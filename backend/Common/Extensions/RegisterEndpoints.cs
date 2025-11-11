using System.Reflection;
using System.Text.RegularExpressions;
using Common.Abstractions.Interfaces;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Common.Extensions;

/// <summary>
/// Provides extension methods for registering and mapping endpoints in the application.
/// </summary>
public static class EndpointExtensions
{
    /// <summary>
    /// Registers all endpoints in the current executing assembly that implement <see cref="IEndpoint"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to register the endpoints with.</param>
    /// <returns>The updated <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddEndpoints(this IServiceCollection services)
    {
        services.AddEndpoints(Assembly.GetExecutingAssembly());
        return services;
    }

    /// <summary>
    /// Registers all endpoints in the specified assembly that implement <see cref="IEndpoint"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to register the endpoints with.</param>
    /// <param name="assembly">The assembly to scan for endpoint implementations.</param>
    /// <returns>The updated <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddEndpoints(this IServiceCollection services, Assembly assembly)
    {
        ServiceDescriptor[] serviceDescriptors = assembly
            .DefinedTypes
            .Where(type => type is { IsAbstract: false, IsInterface: false } &&
                           type.IsAssignableTo(typeof(IEndpoint)))
            .Select(type => ServiceDescriptor.Transient(typeof(IEndpoint), type))
            .ToArray();

        services.TryAddEnumerable(serviceDescriptors);

        return services;
    }

    /// <summary>
    /// Maps all registered endpoints that implement <see cref="IEndpoint"/> to the application's routing pipeline based on their versioning.
    /// Endpoints are categorized according to their parent folder names (e.g., V1, V2, etc.).
    /// Each endpoint is automatically mapped under the `/api/{version}` route.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> instance to which the endpoints are mapped.</param>
    /// <returns>The updated <see cref="WebApplication"/> with mapped endpoints.</returns>
    public static IApplicationBuilder MapEndpoints(this WebApplication app)
    {
        IEnumerable<IEndpoint> endpoints = app.Services.GetRequiredService<IEnumerable<IEndpoint>>();

        var groupedEndpoints = endpoints.GroupBy(e
            => GetVersionFromNamespace(e.GetType()));

        foreach (var group in groupedEndpoints)
        {
            string version = group.Key ?? "v1";
            var routeGroup = app.MapGroup($"/api/{version}");

            foreach (var endpoint in group)
            {
                endpoint.MapEndpoint(routeGroup);
            }
        }

        return app;
    }

    private static string? GetVersionFromNamespace(Type type)
    {
        string? ns = type.Namespace;
        if (ns is null) return null;

        var match = Regex.Match(ns, @"\bV(\d+)\b");
        return match.Success ? $"v{match.Groups[1].Value}" : null;
    }
}