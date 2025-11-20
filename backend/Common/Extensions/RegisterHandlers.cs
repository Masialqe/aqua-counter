using System.Reflection;
using Common.Abstractions.Interfaces;
using Features.Auth;
using Features.Auth.TwoFactor;
using Microsoft.Extensions.DependencyInjection.Extensions;
using static Features.Auth.ConfirmEmail;
using static Features.Auth.Enable2fa;
using static Features.Auth.LoginWith2fa;
using static Features.Auth.LogoutUser;
using static Features.Auth.RefreshUser;
using static Features.Auth.TwoFactor.Setup2fa;
using static Features.Users.LoginUser;
using static Features.Users.RegisterUser;

namespace Common.Extensions;

public static class RegisterHandlers
{
    // public static IServiceCollection RegisterEndpointHandlers(this IServiceCollection services)
    // {
    //     var assembly = Assembly.GetExecutingAssembly();

    //     var types = assembly
    //         .GetTypes()
    //         .Where(t => !t.IsAbstract && !t.IsInterface)
    //         .Select(t => new
    //         {
    //             Implementation = t,
    //             Interfaces = t.GetInterfaces().Where(i =>
    //                 i == typeof(IHandler))
    //         })
    //         .Where(x => x.Interfaces.Any());

    //     foreach (var type in types)
    //     {
    //         foreach (var iface in type.Interfaces)
    //         {
    //             services.AddScoped(iface, type.Implementation);
    //         }
    //     }

    //     return services;
    // }

    public static IServiceCollection RegisterEndpointHandlers(this IServiceCollection services)
    {
        //services.RegisterEndpointHandlers(Assembly.GetExecutingAssembly());

        services.AddScoped<RegisterUserHandler>();
        services.AddScoped<LoginUserHandler>();
        services.AddScoped<LoginWith2faHandler>();
        services.AddScoped<LogoutUserHandler>();
        services.AddScoped<RefreshUserHandler>();
        services.AddScoped<ConfirmEmailHandler>();
        services.AddScoped<Setup2faHandler>();
        services.AddScoped<Enable2faHandler>();

        return services;
    }

    // public static IServiceCollection RegisterEndpointHandlers(this IServiceCollection services, Assembly assembly)
    // {
    //     ServiceDescriptor[] serviceDescriptors = [.. assembly
    //         .DefinedTypes
    //         .Where(type => type is { IsAbstract: false, IsInterface: false } &&
    //                        type.IsAssignableTo(typeof(IHandler)))
    //         .Select(type => ServiceDescriptor.Scoped(typeof(IHandler), type))];

    //     services.TryAddEnumerable(serviceDescriptors);

    //     return services;
    // }
}