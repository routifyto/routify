using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Routify.Data;

public static class DependencyInjection
{
    /// <summary>
    /// Injects the Postgres database context.
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <param name="configuration">Configuration</param>
    public static void AddPostgres(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Postgres");
        services.AddDbContextPool<DatabaseContext>(options =>
        {
            options.UseNpgsql(connectionString, pgOptions =>
            {
                pgOptions.MigrationsHistoryTable("routify_migrations_postgres");
                pgOptions.MigrationsAssembly("Routify.Migrations");
            });
        });
    }
}
