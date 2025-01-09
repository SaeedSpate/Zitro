using Zitro.EntityFramwork.Repositories;

namespace Zitro.EntityFramwork.UnitOfWorks;

public interface IUnitOfWork : IDisposable 
{
    Task<int> ComplateAsync();
}
public interface IUnitOfWork<TEntity> :
    IUnitOfWork
    where TEntity : class
{
    IRepository<TEntity> Repository { get; }
}

public interface IUnitOfWork<TEntity, TKey> :
    IUnitOfWork
    where TEntity : class
{
    IRepository<TEntity, TKey> Repository { get; }
}