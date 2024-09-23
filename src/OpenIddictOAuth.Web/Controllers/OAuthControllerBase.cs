using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using OpenIddictOAuth.Infrastructure.Exceptions;
using SharedHelper;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace OpenIddictOAuth.Web.Controllers;

public class OAuthControllerBase : Controller
{
    protected readonly IOpenIddictApplicationManager ApplicationManager;

    public OAuthControllerBase(
        IOpenIddictApplicationManager applicationManager
    )
    {
        ApplicationManager = applicationManager;
    }

    protected virtual async Task<object> GetApplicationFromUrlAsync(string returnUrl)
    {
        var clientId = returnUrl?.ExtractTextBetweenPatterns("client_id=", "&");

        if (string.IsNullOrEmpty(clientId))
        {
            return null;
        }

        return await GetApplicationByClientIdAsync(clientId);
    }
    
    protected virtual async Task<object> GetApplicationByClientIdAsync(string clientId)
    {
        var application = await ApplicationManager.FindByClientIdAsync(clientId);

        if (application == null)
        {
            // Redirect To Error Page
            throw new UserFriendlyException("invalid_client_id", "The specified client id is invalid");
        }
        
        return application;
    }
    
    protected virtual void VerifyReturnUrl(string returnUrl)
    {
        if (string.IsNullOrEmpty(returnUrl))
        {
            return;
        }

        if (returnUrl.StartsWith("http") || returnUrl.StartsWith("www"))
        {
            // Redirect To Error Page
            throw new UserFriendlyException("invalid_return_url", "The specified return url is invalid");
        }
    }
    
    protected virtual string RedirectionUrlAfterSignIn(string returnUrl)
    {
        if (string.IsNullOrEmpty(returnUrl))
        {
            return "/";
        }

        return Uri.UnescapeDataString(returnUrl);
    }
    
    protected virtual async Task<bool> HasFormValueAsync(string name)
    {
        if (Request.HasFormContentType)
        {
            var form = await Request.ReadFormAsync();
            if (!string.IsNullOrEmpty(form[name]))
            {
                return true;
            }
        }

        return false;
    }
    
    protected static IEnumerable<string> GetDestinations(Claim claim)
    {
        // Note: by default, claims are NOT automatically included in the access and identity tokens.
        // To allow OpenIddict to serialize them, you must attach them a destination, that specifies
        // whether they should be included in access tokens, in identity tokens or in both.

        switch (claim.Type)
        {
            case Claims.Name or Claims.PreferredUsername:
                yield return Destinations.AccessToken;

                if (claim.Subject.HasScope(Scopes.Profile))
                    yield return Destinations.IdentityToken;

                yield break;

            case Claims.Email:
                yield return Destinations.AccessToken;

                if (claim.Subject.HasScope(Scopes.Email))
                    yield return Destinations.IdentityToken;

                yield break;

            case Claims.Role:
                yield return Destinations.AccessToken;

                if (claim.Subject.HasScope(Scopes.Roles))
                    yield return Destinations.IdentityToken;

                yield break;

            // Never include the security stamp in the access and identity tokens, as it's a secret value.
            case "AspNet.Identity.SecurityStamp": yield break;

            default:
                yield return Destinations.AccessToken;
                yield break;
        }
    }
}