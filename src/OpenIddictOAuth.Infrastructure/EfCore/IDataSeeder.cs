namespace OpenIddictOAuth.Infrastructure.EfCore;

public interface IDataSeeder
{
    Task SeedAllAsync();
}