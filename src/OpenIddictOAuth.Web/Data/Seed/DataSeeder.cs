using Microsoft.AspNetCore.Identity;
using OpenIddict.Abstractions;
using OpenIddictOAuth.Infrastructure.EfCore;

namespace OpenIddictOAuth.Web.Data.Seed;

public class DataSeeder : IDataSeeder
{
    private readonly IOpenIddictApplicationManager _applicationManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;

    public DataSeeder(
        IOpenIddictApplicationManager applicationManager, 
        UserManager<ApplicationUser> userManager, 
        RoleManager<ApplicationRole> roleManager
    )
    {
        _applicationManager = applicationManager;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task SeedAllAsync()
    {
        await SeedRoles();
        await SeedUsers();
        await SeedApplications();
    }

    private async Task SeedApplications()
    {
        var initialApp = InitialData.Applications.First();
        
        if (await _applicationManager.FindByClientIdAsync(initialApp.ClientId) == null)
        {
            await _applicationManager.CreateAsync(initialApp);
        }
    }
    
    private async Task SeedRoles()
    {
        if (await _roleManager.RoleExistsAsync("Admin") == false)
        {
            await _roleManager.CreateAsync(new ApplicationRole("Admin"));
        }

        if (await _roleManager.RoleExistsAsync("User") == false)
        {
            await _roleManager.CreateAsync(new ApplicationRole("User"));
        }
    }

    private async Task SeedUsers()
    {
        // Seed Admin User
        if (await _userManager.FindByNameAsync("admin") == null)
        {
            var result = await _userManager.CreateAsync(InitialData.Users.First(), "123qwe");

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(InitialData.Users.First(), "Admin");
            }
        }
    }
}