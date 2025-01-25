using Microsoft.AspNetCore.Identity;
using OpenIddict.Abstractions;
using OpenIddictOAuth.Infrastructure.EfCore;

namespace OpenIddictOAuth.Web.Data.Seed;

public class DataSeeder : IDataSeeder
{
	private readonly IOpenIddictApplicationManager _applicationManager;
	private readonly UserManager<ApplicationUser> _userManager;
	private readonly RoleManager<ApplicationRole> _roleManager;
	private readonly IWebHostEnvironment _webHostEnvironment;

	public DataSeeder(
		IOpenIddictApplicationManager applicationManager,
		UserManager<ApplicationUser> userManager,
		RoleManager<ApplicationRole> roleManager,
		IWebHostEnvironment webHostEnvironment)
	{
		_applicationManager = applicationManager;
		_userManager = userManager;
		_roleManager = roleManager;
		_webHostEnvironment = webHostEnvironment;
	}

	public async Task SeedAllAsync()
	{
		await SeedRoles();
		await SeedUsers();
		await SeedApplications();
	}

	private async Task SeedApplications()
	{
		var initialApps = InitialData.GetApps(_webHostEnvironment.EnvironmentName == "Production");

		foreach (var initialApp in initialApps)
		{
			if (await _applicationManager.FindByClientIdAsync(initialApp.ClientId!) == null)
			{
				await _applicationManager.CreateAsync(initialApp);
			}
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