using Microsoft.EntityFrameworkCore;

namespace Routify.Data;

public class DatabaseContext(DbContextOptions<DatabaseContext> options) : DbContext(options)
{
    
}
