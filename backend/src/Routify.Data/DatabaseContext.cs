using Microsoft.EntityFrameworkCore;
using Routify.Data.Models;

namespace Routify.Data;

public class DatabaseContext(
    DbContextOptions<DatabaseContext> options) 
    : DbContext(options)
{
    public DbSet<User> Users { get; init; }

    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        User.OnModelCreating(modelBuilder);
    }
}
