using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using OpenIddictOAuth.Web.Data;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace OpenIddictOAuth.Web.Controllers;

[Route("connect/userinfo")]
[Authorize(AuthenticationSchemes = OpenIddictServerAspNetCoreDefaults.AuthenticationScheme)]
public class UserInfoController : OAuthControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UserInfoController(
        IOpenIddictApplicationManager applicationManager, 
        UserManager<ApplicationUser> userManager
    ) : base(applicationManager)
    {
        _userManager = userManager;
    }
    
    [HttpGet, HttpPost, Produces("application/json")]
    public async Task<IActionResult> Userinfo()
    {
        var user = await _userManager.FindByIdAsync(User.GetClaim(Claims.Subject));
        if (user is null)
        {
            return Challenge(
                authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                properties: new AuthenticationProperties(new Dictionary<string, string?>
                {
                    [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidToken,
                    [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
                        "The specified access token is bound to an account that no longer exists."
                }));
        }

        var claims = new Dictionary<string, object>(StringComparer.Ordinal)
        {
            // Note: the "sub" claim is a mandatory claim and must be included in the JSON response.
            [Claims.Subject] = await _userManager.GetUserIdAsync(user)
        };

        if (User.HasScope(Scopes.Profile))
        {
            claims[Claims.PreferredUsername] = user.UserName;
            claims[Claims.GivenName] = user.FirstName;
            claims[Claims.FamilyName] = user.LastName;
        }

        if (User.HasScope(Scopes.Email))
        {
            claims[Claims.Email] = user.Email;
            claims[Claims.EmailVerified] = user.EmailConfirmed;
        }

        if (User.HasScope(Scopes.Phone))
        {
            claims[Claims.PhoneNumber] = user.PhoneNumber;
            claims[Claims.PhoneNumberVerified] = user.PhoneNumberConfirmed;
        }

        if (User.HasScope(Scopes.Roles))
        {
            claims[Claims.Role] = await _userManager.GetRolesAsync(user);
        }

        // Note: the complete list of standard claims supported by the OpenID Connect specification
        // can be found here: http://openid.net/specs/openid-connect-core-1_0.html#StandardClaims

        return Ok(claims);
    }
}