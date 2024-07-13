using Microsoft.EntityFrameworkCore;
using Routify.Data.Models;

namespace Routify.Data;

public class DatabaseContext(
    DbContextOptions<DatabaseContext> options) 
    : DbContext(options)
{
    public DbSet<User> Users { get; init; }
    public DbSet<App> Apps { get; init; }
    public DbSet<AppUser> AppUsers { get; init; }
    public DbSet<AppProvider> AppProviders { get; init; }
    public DbSet<Route> Routes { get; init; }
    public DbSet<RouteProvider> RouteProviders { get; init; }
    public DbSet<CompletionLog> CompletionLogs { get; init; }
    public DbSet<ApiKey> ApiKeys { get; init; }

    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        User.OnModelCreating(modelBuilder);
        App.OnModelCreating(modelBuilder);
        AppUser.OnModelCreating(modelBuilder);
        AppProvider.OnModelCreating(modelBuilder);
        Route.OnModelCreating(modelBuilder);
        RouteProvider.OnModelCreating(modelBuilder);
        CompletionLog.OnModelCreating(modelBuilder);
        ApiKey.OnModelCreating(modelBuilder);
    }
}
