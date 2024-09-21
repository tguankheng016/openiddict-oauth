using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace OpenIddictOAuth.Web.Data;

// Used For dotnet ef add or update migration only
public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<ApplicationDbContext>();

        builder.UseNpgsql("Server=localhost;Port=5433;Database=OpeniddictOAuthDb;User Id=postgres;Password=myStong_Password123#;Include Error Detail=true")
            .UseSnakeCaseNamingConvention();
        
        return new ApplicationDbContext(builder.Options);
    }
}