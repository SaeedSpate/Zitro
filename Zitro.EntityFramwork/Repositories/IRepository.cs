using JetBrains.Annotations;

namespace Zitro.EntityFramwork.Repositories;

public interface IRepository { }
public interface IRepository<TEntity> :
    IRepository
    where TEntity : class
{
    Task<List<TEntity>> GetListAsync(CancellationToken cancellationToken = default);
    Task<long> GetCountAsync(CancellationToken cancellationToken = default);

    [NotNull]
    Task<TEntity> InsertAsync([NotNull] TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default);

    [NotNull]
    Task<TEntity> UpdateAsync([NotNull] TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default);
}

public interface IRepository<TEntity, TKey> : 
    IRepository<TEntity>
    where TEntity : class
{

    [NotNull]
    Task<TEntity> GetAsync(TKey id, CancellationToken cancellationToken = default);
    Task DeleteAsync(TKey id, bool autoSave = false, CancellationToken cancellationToken = default);
}
