using Microsoft.AspNetCore.Identity;

namespace OpenIddictOAuth.Web.Data;

public class ApplicationRole : IdentityRole<Guid>
{
    public ApplicationRole()
    {
    }

    public ApplicationRole(string roleName) : this()
    {
        Name = roleName;
    }
}