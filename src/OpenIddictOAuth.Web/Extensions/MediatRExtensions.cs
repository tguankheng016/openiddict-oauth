using MediatR;
using OpenIddictOAuth.Infrastructure.EfCore;
using OpenIddictOAuth.Infrastructure.Logging;
using OpenIddictOAuth.Infrastructure.Validation;

namespace OpenIddictOAuth.Web.Extensions;

public static class MediatRExtensions
{
    public static IServiceCollection AddCustomMediatR(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(Program).Assembly));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(EFTransactionBehavior<,>));

        return services;
    }
}