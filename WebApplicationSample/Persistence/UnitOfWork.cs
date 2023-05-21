using WebApplicationSample.Abstraction;

namespace WebApplicationSample.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _appDbContext;
    private readonly IServiceProvider _provider;
    private readonly Dictionary<Type, object> _dictionary;
    private readonly object _lock = new object();

    public UnitOfWork(AppDbContext appDbContext, IServiceProvider provider)
    {
        _appDbContext = appDbContext;
        _provider = provider;
        _dictionary = new Dictionary<Type, object>();
    }

    public TRepository GetRepositoryOf<TRepository>()
        where TRepository : IRepositoryMarker
    {
        lock (_lock)
        {
            var type = typeof(TRepository);

            if (!_dictionary.TryGetValue(type, out var repo))
            {
                var types = GetType().Assembly.GetTypes();
                var implementationType = types.Single(t => t.IsClass && t.IsAssignableTo(type));
                repo = ActivatorUtilities.CreateInstance(_provider, implementationType, _appDbContext);
                
                if (repo is null)
                {
                    throw new InvalidOperationException(
                        $"Cannot create repository from type {implementationType.FullName}");
                }

                _dictionary.Add(type, repo);
            }

            return (TRepository) repo;
        }
    }

    public Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        return _appDbContext.Database.BeginTransactionAsync(cancellationToken);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _appDbContext.SaveChangesAsync(cancellationToken);
    }

    public Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        return _appDbContext.Database.CommitTransactionAsync(cancellationToken);
    }

    public Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        return _appDbContext.Database.RollbackTransactionAsync(cancellationToken);
    }
}