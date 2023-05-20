using WebApplication.Domain;

namespace WebApplication.Abstraction;

public interface IUnitOfWork
{
    IRepository<TEntity, TKey> GetRepositoryOf<TEntity, TKey>()
        where TEntity : Entity<TKey>
        where TKey : struct, IEquatable<TKey>;
    
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}