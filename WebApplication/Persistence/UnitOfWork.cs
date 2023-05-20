using WebApplication.Abstraction;
using WebApplication.Domain;

namespace WebApplication.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppContext _appContext;
    private readonly Dictionary<Type, object> _dictionary;
    private readonly object _lock = new object();

    public UnitOfWork(AppContext appContext)
    {
        _appContext = appContext;
        _dictionary = new Dictionary<Type, object>();
    }

    public IRepository<TEntity, TKey> GetRepositoryOf<TEntity, TKey>() where TEntity : Entity<TKey>
        where TKey : struct, IEquatable<TKey>
    {
        lock (_lock)
        {
            var type = typeof(IRepository<TEntity, TKey>);
            if (!_dictionary.TryGetValue(type, out var repo))
            {
                repo = AppDomain.CurrentDomain.GetAssemblies()
                    .Select(t => t.GetType())
                    .Single(t => type.IsAssignableFrom(t));
                _dictionary.Add(type, repo);
            }

            return (IRepository<TEntity, TKey>)repo;
        }
    }

    public Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        return _appContext.Database.BeginTransactionAsync(cancellationToken);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _appContext.SaveChangesAsync(cancellationToken);
    }

    public Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        return _appContext.Database.CommitTransactionAsync(cancellationToken);
    }

    public Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        return _appContext.Database.RollbackTransactionAsync(cancellationToken);
    }
}