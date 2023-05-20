using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using WebApplication.Abstraction;
using WebApplication.Domain;

namespace WebApplication.Persistence.Repositories;

public abstract class BaseRepository<TEntity, TKey>:IRepository<TEntity, TKey>
    where TEntity : Entity<TKey>
    where TKey : struct, IEquatable<TKey>
{
    private readonly AppContext _appContext;

    protected BaseRepository(AppContext appContext)
    {
        _appContext = appContext;
    }


    public Task InsertAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        return _appContext.AddAsync(entity, cancellationToken).AsTask();
    }

    public Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        _appContext.Entry(entity).State = EntityState.Modified;
        return Task.CompletedTask;
    }

    public Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        _appContext.Entry(entity).State = EntityState.Deleted;
        return Task.CompletedTask;
    }

    public Task<TEntity> GetByIdAsync(TKey id, CancellationToken cancellationToken = default)
    {
        return _appContext.Set<TEntity>().SingleAsync(x => x.Id.Equals(id), cancellationToken: cancellationToken);
    }

    public Task<TEntity?> GetByIdAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default)
    {
        return _appContext.Set<TEntity>().FirstOrDefaultAsync(expression, cancellationToken);
    }

    public Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default)
    {
        return _appContext.Set<TEntity>().AnyAsync(expression, cancellationToken);
    }

    public Task<long> CountAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default)
    {
        return _appContext.Set<TEntity>().LongCountAsync(expression, cancellationToken);
    }

    public Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default)
    {
        return _appContext.Set<TEntity>().Where(expression).ToListAsync(cancellationToken);
    }
    public Task<List<TEntity>> GetListAsNoTrackingAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default)
    {
        return _appContext.Set<TEntity>().AsNoTracking().Where(expression).ToListAsync(cancellationToken);
    }
}