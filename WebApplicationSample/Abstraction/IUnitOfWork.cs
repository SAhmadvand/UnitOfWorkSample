namespace WebApplicationSample.Abstraction;

public interface IUnitOfWork
{
    TRepository GetRepositoryOf<TRepository>()
        where TRepository : IRepositoryMarker;

    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}