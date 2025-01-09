using Zitro.EntityFramwork.EntityFramework;
using Zitro.EntityFramwork.Repositories;

namespace Zitro.EntityFramwork.UnitOfWorks;

public class UnitOfWork : IUnitOfWork
{
    protected ZitroDbContext _dbContext;

    public UnitOfWork(ZitroDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SaveChangeAsync()
    {
        await _dbContext.SaveChangesAsync();
    }

    public async void Dispose()
    {
        await _dbContext.DisposeAsync();
    }

    public async Task<int> ComplateAsync()
    {
        return await _dbContext.SaveChangesAsync();
    }
}

public class UnitOfWork<TEntity> :
    UnitOfWork,
    IUnitOfWork<TEntity>
    where TEntity : class
{
    public IRepository<TEntity> Repository { get; private set; }

    public UnitOfWork(ZitroDbContext dbContext) : base(dbContext)
    {
        Repository = new Repository<TEntity>(dbContext);
    }
}

public class UnitOfWork<TEntity, TKey> :
    UnitOfWork,
    IUnitOfWork<TEntity, TKey>
    where TEntity : class
{
    public IRepository<TEntity, TKey> Repository {  get; private set; }

    public UnitOfWork(ZitroDbContext dbContext) : base(dbContext)
    {
        Repository = new Repository<TEntity, TKey>(dbContext);
    }
}