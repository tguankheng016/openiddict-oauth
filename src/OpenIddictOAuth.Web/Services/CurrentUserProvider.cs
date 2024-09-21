using System.Security.Claims;
using OpenIddictOAuth.Infrastructure.Dependency;

namespace OpenIddictOAuth.Web.Services;

public interface ICurrentUserProvider : IScopedDependency
{
    long? GetCurrentUserId();
}

public class CurrentUserProvider : ICurrentUserProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }


    public long? GetCurrentUserId()
    {
        var nameIdentifier = _httpContextAccessor?.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        
        if (!long.TryParse(nameIdentifier, out var userId))
        {
            return null;
        }

        return userId;
    }
}