namespace WebApplicationSample.Abstraction;

public delegate Task TransactionDelegate(CancellationToken cancellationToken = default);

public interface IUnitOfWork
{
    TRepository GetRepository<TRepository>()
        where TRepository : IRepositoryMarker;

    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(TransactionDelegate action, CancellationToken cancellationToken = default);
}