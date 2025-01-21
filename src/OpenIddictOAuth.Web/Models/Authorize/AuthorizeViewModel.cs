namespace OpenIddictOAuth.Web.Models.Authorize;

public class AuthorizeViewModel
{
    public string? ClientName { get; set; }

    public string? ClientLogoUrl { get; set; }

    public string? Username { get; set; }

    public string? UserProfileImageUrl { get; set; }

    public bool AllowRememberConsent { get; set; }

    public IEnumerable<ScopeViewModel>? Scopes { get; set; }
}

public class AuthorizeInputViewModel
{
    public IEnumerable<string>? ScopesConsented { get; set; }

    public bool RememberConsent { get; set; }

    public string? ReturnUrl { get; set; }
}