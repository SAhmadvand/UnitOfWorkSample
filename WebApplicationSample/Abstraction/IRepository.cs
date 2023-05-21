using System.Linq.Expressions;
using WebApplicationSample.Domain;

namespace WebApplicationSample.Abstraction;

public interface IRepositoryMarker
{
}

public interface IRepository<TEntity, in TKey> : IRepositoryMarker
    where TEntity : Entity<TKey>
    where TKey : struct, IEquatable<TKey>
{
    Task InsertAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task<TEntity> GetByIdAsync(TKey id, CancellationToken cancellationToken = default);

    Task<TEntity?> GetByIdAsync(Expression<Func<TEntity, bool>> expression,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default);
    Task<long> CountAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default);

    Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> expression,
        CancellationToken cancellationToken = default);

    Task<List<TEntity>> GetListAsNoTrackingAsync(Expression<Func<TEntity, bool>> expression,
        CancellationToken cancellationToken = default);
}