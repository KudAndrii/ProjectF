using Microsoft.EntityFrameworkCore;
using ProjectF.DataAccess.Entities;

namespace ProjectF.DataAccess.DbContexts;

public class SqlDbContext(DbContextOptions<SqlDbContext> options) : DbContext(options)
{
    public DbSet<MoviesQueryEntity> Users { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(typeof(SqlDbContext).Assembly);
    }
}