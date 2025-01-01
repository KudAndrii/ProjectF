using ProjectF.DataAccess.DbContexts;
using ProjectF.DataAccess.Entities;
using ProjectF.DataAccess.Interfaces;

namespace ProjectF.DataAccess.Repositories;

public class MoviesQueriesRepository(SqlDbContext context) : IMoviesQueriesRepository
{
    public async Task AddAsync(MoviesQueryEntity entity, CancellationToken cancellationToken)
    {
        await context.MoviesQueries.AddAsync(entity, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }
}