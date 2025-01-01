using ProjectF.DataAccess.Entities;

namespace ProjectF.DataAccess.Interfaces;

public interface IMoviesQueriesRepository
{
    Task AddAsync(MoviesQueryEntity entity, CancellationToken cancellationToken);
}