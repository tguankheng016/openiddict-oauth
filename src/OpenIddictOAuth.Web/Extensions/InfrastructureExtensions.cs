using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenIddictOAuth.Infrastructure.Dependency;
using OpenIddictOAuth.Infrastructure.EfCore;
using OpenIddictOAuth.Infrastructure.Logging;
using OpenIddictOAuth.Infrastructure.Postgres;
using OpenIddictOAuth.Infrastructure.ProblemDetails;
using OpenIddictOAuth.Web.Data;
using OpenIddictOAuth.Web.Data.Seed;
using Serilog;

namespace OpenIddictOAuth.Web.Extensions;

public static class InfrastructureExtensions
{
    public static WebApplicationBuilder AddInfrastructure(this WebApplicationBuilder builder)
    {
        var assembly = typeof(Program).Assembly;
        var configuration = builder.Configuration;
        var env = builder.Environment;

        builder.Services.AddDefaultDependencyInjection(assembly);
        builder.Services.AddScoped<IDataSeeder, DataSeeder>();
        
        builder.Services.Configure<ApiBehaviorOptions>(options =>
        {
            // Used for preventing return 400 error if model state is invalid
            options.SuppressModelStateInvalidFilter = true;
        });
        
        builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
        builder.Services.AddNpgDbContext<ApplicationDbContext>();
        builder.AddCustomSerilog(env);
        builder.Services.AddCustomMediatR();
        builder.Services.AddValidatorsFromAssembly(assembly);
        builder.Services.AddProblemDetails();
        builder.Services.AddAutoMapper(assembly);
        
        builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(config =>
            {
                config.Password.RequiredLength = 6;
                config.Password.RequireDigit = false;
                config.Password.RequireNonAlphanumeric = false;
                config.Password.RequireUppercase = false;
            }
        )
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

        builder.Services.AddCustomOpenIddict();

        return builder;
    }

    public static WebApplication UseInfrastructure(this WebApplication app)
    {
        var env = app.Environment;
        
        app.UseStatusCodePagesWithRedirects("~/Error?statusCode={0}");
        
        app.UseExceptionHandler("/Error");

        app.UseStaticFiles();

        app.UseRouting();
        
        app.UseForwardedHeaders();
        
        app.UseCustomProblemDetails();

        app.UseSerilogRequestLogging(options =>
        {
            options.EnrichDiagnosticContext = LogEnrichHelper.EnrichFromRequest;
        });
        
        app.UseAuthentication();

        app.UseAuthorization();
        
        app.UseMigration<ApplicationDbContext>(env);
        
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapDefaultControllerRoute();
        });

        return app;
    }
}