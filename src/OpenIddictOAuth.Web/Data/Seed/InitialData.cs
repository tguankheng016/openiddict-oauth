using OpenIddict.Abstractions;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace OpenIddictOAuth.Web.Data.Seed;

public static class InitialData
{
	public static List<ApplicationUser> Users { get; }
	public static List<OpenIddictApplicationDescriptor> Applications { get; }

	static InitialData()
	{
		Users = new List<ApplicationUser>
		{
			new ApplicationUser
			{
				Id = new Guid("c5fb6ddb-3551-487b-a38a-686d27376c30"),
				FirstName = "Admin",
				LastName = "Tan",
				UserName = "admin",
				Email = "admin@testgk.com",
				SecurityStamp = Guid.NewGuid().ToString()
			}
		};
		Applications = new List<OpenIddictApplicationDescriptor>
		{
			new OpenIddictApplicationDescriptor
			{
				ClientId = "dotnet-commerce",
				ClientSecret = "901564A5-E7FE-42CB-B10D-61EF6A8F3654",
				ConsentType = ConsentTypes.Explicit,
				DisplayName = "Dotnet Commerce Micro",
				RedirectUris =
				{
					new Uri("http://localhost:4200/account/callback/login"),
					new Uri("http://localhost:5173/account/callback/login")
				},
				PostLogoutRedirectUris =
				{
					new Uri("https://localhost:4200/account/callback/logout"),
					new Uri("https://localhost:5173/account/callback/logout")
				},
				Permissions =
				{
					Permissions.Endpoints.Authorization,
					Permissions.Endpoints.Logout,
					Permissions.Endpoints.Token,
					Permissions.GrantTypes.AuthorizationCode,
					Permissions.ResponseTypes.Code,
					Permissions.Scopes.Email,
					Permissions.Scopes.Profile,
					Permissions.Scopes.Roles
				}
			},
			new OpenIddictApplicationDescriptor
			{
				ClientId = "go-commerce",
				ClientSecret = "901564A5-E7FE-42CB-B10D-61EF6A8F3654",
				ConsentType = ConsentTypes.Explicit,
				DisplayName = "Go Commerce Micro",
				RedirectUris =
				{
					new Uri("http://localhost:4200/account/callback/login"),
					new Uri("http://localhost:5173/account/callback/login")
				},
				PostLogoutRedirectUris =
				{
					new Uri("https://localhost:4200/account/callback/logout"),
					new Uri("https://localhost:5173/account/callback/logout")
				},
				Permissions =
				{
					Permissions.Endpoints.Authorization,
					Permissions.Endpoints.Logout,
					Permissions.Endpoints.Token,
					Permissions.GrantTypes.AuthorizationCode,
					Permissions.ResponseTypes.Code,
					Permissions.Scopes.Email,
					Permissions.Scopes.Profile,
					Permissions.Scopes.Roles
				}
			}
		};
	}

	public static List<OpenIddictApplicationDescriptor> GetApps(bool isProd = false)
	{
		if (isProd)
		{
			return new List<OpenIddictApplicationDescriptor>
			{
				new OpenIddictApplicationDescriptor
				{
					ClientId = "dotnet-commerce",
					ClientSecret = "901564A5-E7FE-42CB-B10D-61EF6A8F3654",
					ConsentType = ConsentTypes.Explicit,
					DisplayName = "Dotnet Commerce Micro",
					RedirectUris =
					{
						new Uri("https://eportal.gktan.com/account/callback/login"),
						new Uri("https://eshop.gktan.com/account/callback/login")
					},
					PostLogoutRedirectUris =
					{
						new Uri("https://eportal.gktan.com/account/callback/logout"),
						new Uri("https://eshop.gktan.com/account/callback/logout")
					},
					Permissions =
					{
						Permissions.Endpoints.Authorization,
						Permissions.Endpoints.Logout,
						Permissions.Endpoints.Token,
						Permissions.GrantTypes.AuthorizationCode,
						Permissions.ResponseTypes.Code,
						Permissions.Scopes.Email,
						Permissions.Scopes.Profile,
						Permissions.Scopes.Roles
					}
				},
				new OpenIddictApplicationDescriptor
				{
					ClientId = "go-commerce",
					ClientSecret = "901564A5-E7FE-42CB-B10D-61EF6A8F3654",
					ConsentType = ConsentTypes.Explicit,
					DisplayName = "Go Commerce Micro",
					RedirectUris =
					{
						new Uri("https://goportal.gktan.com/account/callback/login"),
						new Uri("https://goshop.gktan.com/account/callback/login"),
					},
					PostLogoutRedirectUris =
					{
						new Uri("https://goportal.gktan.com/account/callback/logout"),
						new Uri("https://goshop.gktan.com/account/callback/logout")
					},
					Permissions =
					{
						Permissions.Endpoints.Authorization,
						Permissions.Endpoints.Logout,
						Permissions.Endpoints.Token,
						Permissions.GrantTypes.AuthorizationCode,
						Permissions.ResponseTypes.Code,
						Permissions.Scopes.Email,
						Permissions.Scopes.Profile,
						Permissions.Scopes.Roles
					}
				}
			};
		}

		return new List<OpenIddictApplicationDescriptor>
		{
			new OpenIddictApplicationDescriptor
			{
				ClientId = "dotnet-commerce",
				ClientSecret = "901564A5-E7FE-42CB-B10D-61EF6A8F3654",
				ConsentType = ConsentTypes.Explicit,
				DisplayName = "Dotnet Commerce Micro",
				RedirectUris =
				{
					new Uri("http://localhost:4200/account/callback/login"),
					new Uri("http://localhost:5173/account/callback/login")
				},
				PostLogoutRedirectUris =
				{
					new Uri("https://localhost:4200/account/callback/logout"),
					new Uri("https://localhost:5173/account/callback/logout")
				},
				Permissions =
				{
					Permissions.Endpoints.Authorization,
					Permissions.Endpoints.Logout,
					Permissions.Endpoints.Token,
					Permissions.GrantTypes.AuthorizationCode,
					Permissions.ResponseTypes.Code,
					Permissions.Scopes.Email,
					Permissions.Scopes.Profile,
					Permissions.Scopes.Roles
				}
			},
			new OpenIddictApplicationDescriptor
			{
				ClientId = "go-commerce",
				ClientSecret = "901564A5-E7FE-42CB-B10D-61EF6A8F3654",
				ConsentType = ConsentTypes.Explicit,
				DisplayName = "Go Commerce Micro",
				RedirectUris =
				{
					new Uri("http://localhost:4200/account/callback/login"),
					new Uri("http://localhost:5173/account/callback/login")
				},
				PostLogoutRedirectUris =
				{
					new Uri("https://localhost:4200/account/callback/logout"),
					new Uri("https://localhost:5173/account/callback/logout")
				},
				Permissions =
				{
					Permissions.Endpoints.Authorization,
					Permissions.Endpoints.Logout,
					Permissions.Endpoints.Token,
					Permissions.GrantTypes.AuthorizationCode,
					Permissions.ResponseTypes.Code,
					Permissions.Scopes.Email,
					Permissions.Scopes.Profile,
					Permissions.Scopes.Roles
				}
			}
		};
	}
}