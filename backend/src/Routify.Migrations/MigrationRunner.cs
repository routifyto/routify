using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Routify.Data;

namespace Routify.Migrations;

internal class MigrationRunner(IServiceScopeFactory serviceScopeFactory)
{
    public async Task RunMigrationsAsync()
    {
        using var scope = serviceScopeFactory.CreateScope();
        var databaseContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
        await databaseContext.Database.MigrateAsync();
    }
}