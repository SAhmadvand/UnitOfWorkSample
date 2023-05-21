using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;
using WebApplicationSample.Abstraction;

namespace WebApplicationSample.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _appDbContext;
    private readonly IServiceProvider _provider;
    [ConcurrencyCheck] private readonly ConcurrentDictionary<Type, object> _dictionary;
    private readonly object _lock = new object();

    public UnitOfWork(AppDbContext appDbContext, IServiceProvider provider)
    {
        _appDbContext = appDbContext;
        _provider = provider;
        _dictionary = new ConcurrentDictionary<Type, object>();
    }


    public TRepository GetRepository<TRepository>()
        where TRepository : IRepositoryMarker
    {
        var type = typeof(TRepository);
        if (_dictionary.GetOrAdd(type, (k) =>
            {
                var types = GetType().Assembly.GetTypes();
                var implementationType = types.Single(t => t.IsClass && t.IsAssignableTo(type));
                return ActivatorUtilities.CreateInstance(_provider, implementationType, _appDbContext);
            }) is TRepository repository) return repository;

        throw new Exception();
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

    public async Task TransactionAsync(TransactionDelegate action, CancellationToken cancellationToken = default)
    {
        try
        {
            await BeginTransactionAsync(cancellationToken);
            await action(cancellationToken);
            await SaveChangesAsync(cancellationToken);
            await CommitTransactionAsync(cancellationToken);
        }
        catch
        {
            await RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }
}