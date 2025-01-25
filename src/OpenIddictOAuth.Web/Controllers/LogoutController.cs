using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using OpenIddictOAuth.Infrastructure.Exceptions;
using OpenIddictOAuth.Web.Data;
using OpenIddictOAuth.Web.Services;

namespace OpenIddictOAuth.Web.Controllers;

[Route("connect/logout")]
[ApiExplorerSettings(IgnoreApi = true)]
public class LogoutController : OAuthControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly ICurrentUserProvider _currentUserProvider;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public LogoutController(
        IOpenIddictApplicationManager applicationManager,
        SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager,
        IConfiguration configuration,
        ICurrentUserProvider currentUserProvider) : base(applicationManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _configuration = configuration;
        _currentUserProvider = currentUserProvider;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var request = HttpContext.GetOpenIddictServerRequest()
            ?? throw new BadRequestException("The OpenID Connect request cannot be retrieved.");

        var result = await HttpContext.AuthenticateAsync(IdentityConstants.ApplicationScheme);

        if (result.Principal == null)
        {
            return RedirectSignOutResult();
        }

        var user = await _userManager.GetUserAsync(result.Principal);

        if (user == null)
        {
            return RedirectSignOutResult();
        }

        // Retrieve the application details from the database.
        var application = await ApplicationManager.FindByClientIdAsync(request.ClientId) ??
            throw new InvalidOperationException("DetailsConcerningTheCallingClientApplicationCannotBeFound");

        var userProfileUrl = _configuration["UiAvatars:BaseUrl"];

        ViewBag.Username = user.Email;
        ViewBag.UserProfileImageUrl = userProfileUrl;
        ViewBag.ApplicationName = await ApplicationManager.GetLocalizedDisplayNameAsync(application);
        //ViewBag.ApplicationLogoUrl = application.LogoUri;

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        var userId = _currentUserProvider.GetCurrentUserId();

        if (userId == null) return RedirectSignOutResult();

        var user = await _userManager.FindByIdAsync(userId.Value.ToString());

        if (user != null)
        {
            await _userManager.UpdateSecurityStampAsync(user);
            await _signInManager.SignOutAsync();
        }

        return RedirectSignOutResult();
    }

    private IActionResult RedirectSignOutResult()
    {
        return SignOut(
            authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
            properties: new AuthenticationProperties
            {
                RedirectUri = "/Login"
            });
    }
}