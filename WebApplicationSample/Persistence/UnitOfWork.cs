using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;
using WebApplicationSample.Abstraction;

namespace WebApplicationSample.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _appDbContext;
    private readonly IServiceProvider _provider;
    private readonly ConcurrentDictionary<Type, object> _dictionary;

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
        var repository = _dictionary.GetOrAdd(type, (k) =>
        {
            var implementationType = AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(t => t.GetTypes())
                .Single(t => t.IsClass && t.IsAssignableTo(type));
            return ActivatorUtilities.CreateInstance(_provider, implementationType, _appDbContext);
        });
        return (TRepository)repository;
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

    public async Task CommitTransactionAsync(TransactionDelegate action, CancellationToken cancellationToken = default)
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

    public Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        return _appDbContext.Database.RollbackTransactionAsync(cancellationToken);
    }
}