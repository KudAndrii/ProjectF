using ProjectF.DataAccess.DbContexts;
using ProjectF.DataAccess.Interfaces;

namespace ProjectF.DataAccess.Repositories;

public class MoviesQueriesRepository(SqlDbContext context) : IMoviesQueriesRepository
{
    
}