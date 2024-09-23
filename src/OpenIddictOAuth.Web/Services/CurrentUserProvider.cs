using System.Security.Claims;
using OpenIddictOAuth.Infrastructure.Dependency;

namespace OpenIddictOAuth.Web.Services;

public interface ICurrentUserProvider : IScopedDependency
{
    Guid? GetCurrentUserId();
}

public class CurrentUserProvider : ICurrentUserProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }


    public Guid? GetCurrentUserId()
    {
        var nameIdentifier = _httpContextAccessor?.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        
        if (!Guid.TryParse(nameIdentifier, out var userId))
        {
            return null;
        }

        return userId;
    }
}