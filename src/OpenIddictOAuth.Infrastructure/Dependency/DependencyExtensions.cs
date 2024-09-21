using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace OpenIddictOAuth.Infrastructure.Dependency;

public static class DependencyExtensions
{
    public static IServiceCollection AddDefaultDependencyInjection(
        this IServiceCollection services, params Assembly[] assemblies)
    {
        services.Scan(i =>
            i.FromAssemblies(assemblies)
                .AddClasses(c => c.AssignableTo<ITransientDependency>())
                .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                .AsImplementedInterfaces()
                .WithTransientLifetime()
                .AddClasses(c => c.AssignableTo<IScopedDependency>())
                .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                .AsImplementedInterfaces()
                .WithScopedLifetime()
                .AddClasses(c => c.AssignableTo<ISingletonDependency>())
                .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                .AsImplementedInterfaces()
                .WithSingletonLifetime()
        );
        
        // Common Service
        
        return services;
    }
}