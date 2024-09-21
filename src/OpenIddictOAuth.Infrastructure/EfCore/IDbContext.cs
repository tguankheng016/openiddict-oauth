using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace OpenIddictOAuth.Infrastructure.EfCore;

public interface IDbContext
{
    DbSet<TEntity> Set<TEntity>() where TEntity : class;
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
    
    IExecutionStrategy CreateExecutionStrategy();
    
    Task ExecuteTransactionalAsync(CancellationToken cancellationToken = default);

    bool HasActiveTransaction { get; }
}