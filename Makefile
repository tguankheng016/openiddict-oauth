# Add Migrations
MIGRATION_NAME = Test
MIGRATION_PROJECT = src/OpenIddictOAuth.Web/OpenIddictOAuth.Web.csproj
STARTUP_PROJECT = src/OpenIddictOAuth.Web/OpenIddictOAuth.Web.csproj
DBCONTEXT_WITH_NAMESPACE = OpenIddictOAuth.Web.Data.ApplicationDbContext
OUTPUT_DIR = Data/Migrations
run_migration:
	dotnet ef migrations add --project $(MIGRATION_PROJECT) --startup-project $(STARTUP_PROJECT) --context $(DBCONTEXT_WITH_NAMESPACE) --configuration Debug --verbose $(MIGRATION_NAME) --output-dir $(OUTPUT_DIR)