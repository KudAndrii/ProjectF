using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using ProjectF.DataAccess.DbContexts;
using ProjectF.DataAccess.Interfaces;
using ProjectF.DataAccess.Repositories;

namespace ProjectF.DataAccess;

public class UnitOfWork(IDbContextFactory<SqlDbContext> dbContextFactory) : IDisposable
{
    private bool _disposed;
    private readonly SqlDbContext _context = dbContextFactory.CreateDbContext();
    private IMoviesQueriesRepository? _moviesQueriesRepository;

    public IMoviesQueriesRepository MoviesQueriesRepository =>
        _moviesQueriesRepository ??= new MoviesQueriesRepository(_context);

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken) =>
        _context.SaveChangesAsync(cancellationToken);

    public async Task<IDbTransaction> BeginTransactionAsync(CancellationToken cancellationToken) =>
        (await _context.Database.BeginTransactionAsync(cancellationToken)).GetDbTransaction();

    private void Dispose(bool disposing)
    {
        if (_disposed) return;

        if (disposing)
        {
            _context?.Dispose();
            _moviesQueriesRepository = null;
        }

        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~UnitOfWork()
    {
        Dispose(false);
    }
}