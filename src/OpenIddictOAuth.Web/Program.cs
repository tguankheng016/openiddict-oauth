using OpenIddictOAuth.Web.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseDefaultServiceProvider((context, options) =>
{
    // Service provider validation
    // ref: https://andrewlock.net/new-in-asp-net-core-3-service-provider-validation/
    // Used for check got any DI misconfigured
    options.ValidateScopes = context.HostingEnvironment.IsDevelopment() || context.HostingEnvironment.IsStaging();
    options.ValidateOnBuild = true;
});

builder.AddInfrastructure();

var app = builder.Build();

app.UseInfrastructure();

app.Run();