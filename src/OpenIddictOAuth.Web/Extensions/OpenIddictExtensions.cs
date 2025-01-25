using OpenIddict.Server;
using OpenIddictOAuth.Infrastructure.Exceptions;
using OpenIddictOAuth.Web.Data;
using static OpenIddict.Abstractions.OpenIddictConstants;
using static OpenIddict.Server.OpenIddictServerEvents;

namespace OpenIddictOAuth.Web.Extensions;

public static class OpenIddictExtensions
{
    public static IServiceCollection AddCustomOpenIddict(this IServiceCollection services)
    {
        services.AddOpenIddict()
            // Register the OpenIddict core components.
            .AddCore(options =>
            {
                // Configure OpenIddict to use the Entity Framework Core stores and models.
                // Note: call ReplaceDefaultEntities() to replace the default OpenIddict entities.
                options.UseEntityFrameworkCore()
                    .UseDbContext<ApplicationDbContext>();
            })
            .AddServer(options =>
            {
                // Enable the authorization, logout, token and userinfo endpoints.
                options.SetAuthorizationEndpointUris("connect/authorize", "connect/authorize/callback")
                    .SetLogoutEndpointUris("connect/logout")
                    .SetTokenEndpointUris("connect/token")
                    .SetUserinfoEndpointUris("connect/userinfo");

                // Mark the "email", "profile" and "roles" scopes as supported scopes.
                options.RegisterScopes(Scopes.Email, Scopes.Profile, Scopes.Roles);

                // Note: this sample only uses the authorization code flow but you can enable
                // the other flows if you need to support implicit, password or client credentials.
                options.AllowAuthorizationCodeFlow();

                // Custom Event Handler
                options.AddEventHandler<ProcessErrorContext>(builder =>
                {
                    builder.UseSingletonHandler<MyProcessErrorContextHandler>();
                });

                // Register the signing and encryption credentials.
                options.AddDevelopmentEncryptionCertificate()
                    .AddDevelopmentSigningCertificate();

                // Register the ASP.NET Core host and configure the ASP.NET Core-specific options.
                options.UseAspNetCore()
                    .EnableAuthorizationEndpointPassthrough()
                    .EnableLogoutEndpointPassthrough()
                    .EnableTokenEndpointPassthrough()
                    .EnableUserinfoEndpointPassthrough()
                    .EnableStatusCodePagesIntegration()
                    .DisableTransportSecurityRequirement();

                options.SetIssuer(new Uri("http://localhost:41001/"));
            })
            .AddValidation(options =>
            {
                // Import the configuration from the local OpenIddict server instance.
                options.UseLocalServer();

                // Register the ASP.NET Core host.
                options.UseAspNetCore();
            });

        services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = "/login";
            options.ExpireTimeSpan = TimeSpan.FromDays(30);
            options.SlidingExpiration = true;
        });

        return services;
    }

    private class MyProcessErrorContextHandler : IOpenIddictServerHandler<ProcessErrorContext>
    {
        public ValueTask HandleAsync(ProcessErrorContext context)
        {
            if (context.Error != null
                && context.EndpointType != OpenIddictServerEndpointType.Userinfo
                && context.EndpointType != OpenIddictServerEndpointType.Token)
            {
                throw new BadRequestException(context.ErrorDescription ?? "");
            }

            return default;
        }
    }
}