using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OpenIddictOAuth.Infrastructure.Configurations;
using OpenIddictOAuth.Infrastructure.EfCore;

namespace OpenIddictOAuth.Infrastructure.Postgres;

public static class NpgExtensions
{
    public static IServiceCollection AddNpgDbContext<TContext>(
        this IServiceCollection services)
        where TContext : DbContext, IDbContext
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        services.AddValidateOptions<PostgresOptions>();

        services.AddDbContext<TContext>((sp, options) =>
        {
            var postgresOptions = sp.GetRequiredService<PostgresOptions>();

            options.UseNpgsql(
                postgresOptions?.ConnectionString,
                dbOptions =>
                {
                    dbOptions.MigrationsAssembly(typeof(TContext).Assembly.GetName().Name);
                }
            ).UseSnakeCaseNamingConvention();
            // https://github.com/efcore/EFCore.NamingConventions
            
            options.UseOpenIddict();
        });

        services.AddScoped<IDbContext>(provider => provider.GetService<TContext>());

        return services;
    }
}