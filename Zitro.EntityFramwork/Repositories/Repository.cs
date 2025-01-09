using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Zitro.Domain.Catalogs;
using Zitro.EntityFramwork.EntityFramework;

namespace Zitro.EntityFramwork.Repositories;
public class Repository : IRepository 
{
    protected ZitroDbContext _dbContext;
    public Repository(ZitroDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}
public class Repository<TEntity> :
    Repository,
    IRepository<TEntity> where TEntity : class
{
    protected DbSet<TEntity> _entity;
    public Repository(ZitroDbContext dbContext) : base(dbContext)
    {
        _entity = dbContext.Set<TEntity>();
    }

    public virtual async Task<long> GetCountAsync(CancellationToken cancellationToken = default)
    {
        return await _entity.CountAsync(cancellationToken);
    }

    public virtual async Task<List<TEntity>> GetListAsync(CancellationToken cancellationToken = default)
    {
        if(typeof(TEntity) == typeof(Product))
        {
            return await _dbContext.Set<Product>()
                .Include(a => a.Attributes)
                .Include(i => i.ProductImages)
                .ToListAsync() as List<TEntity>;
        }
        else
        {
            return await _entity.ToListAsync(cancellationToken);
        }
    }

    public virtual async Task<TEntity> InsertAsync([NotNull] TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        var newEntity = await _entity.AddAsync(entity, cancellationToken);

        if (autoSave)
        {
            await _dbContext.SaveChangesAsync();
        }

        return newEntity.Entity;
    }

    public virtual async Task<TEntity> UpdateAsync([NotNull] TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        var entry = _dbContext.Entry(entity);
        
        if(entry.State == EntityState.Detached)
        {
            _entity.Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
        }

        if (typeof(TEntity) == typeof(Product))
        {
            var product = entity as Product;

            await _dbContext.Entry(product).Collection(img => img.ProductImages).LoadAsync();
            foreach(var image in product.ProductImages)
            {
                if (!await _dbContext.Set<ProductImage>().AnyAsync(x => x.Id == image.Id))
                {
                    _dbContext.Entry(image).State = EntityState.Added;
                }
                else
                {
                    _dbContext.Entry(image).State = EntityState.Modified;
                }
            }
            
            await _dbContext.Entry(product).Collection(attr => attr.Attributes).LoadAsync();
            foreach(var attribute in product.Attributes)
            {
                if(!await _dbContext.Set<ProductAttribute>().AnyAsync(x => x.Id == attribute.Id))
                {
                    _dbContext.Entry(attribute).State = EntityState.Added;
                }
                else
                {
                    _dbContext.Entry(attribute).State = EntityState.Modified;
                }
            }
        }
        
        var updatedEntity = _entity.Update(entity);

        try
        {
            if (autoSave)
            {
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }
        catch (DbUpdateConcurrencyException ex)
        {
            foreach (var entryEntity in ex.Entries)
            {
                if(entryEntity is TEntity)
                {
                    var databaseEntity = await _entity.FindAsync(entry.Property("Id").CurrentValue, cancellationToken);
                    if (databaseEntity != null)
                    {
                        var databaseValues = entry.GetDatabaseValues();
                        if (databaseValues != null)
                        {
                            entry.OriginalValues.SetValues(databaseValues);
                        }
                    }
                }
            }
        }

        return updatedEntity.Entity;
    }
}

public class Repository<TEntity, TKey> : Repository<TEntity>, IRepository<TEntity, TKey>
    where TEntity : class
{
    public Repository(ZitroDbContext dbContext) : base(dbContext)
    {
    }

    public virtual async Task DeleteAsync(TKey id, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        var entity = await GetAsync(id, cancellationToken: cancellationToken);
        if (entity == null)
        {
            throw new Exception($"Entity with ID: {id}, was not exist!");
        }
        _entity.Remove(entity);
        
        if(autoSave)
            await _dbContext.SaveChangesAsync();
    }

    public virtual async Task<TEntity> GetAsync(TKey id, CancellationToken cancellationToken = default)
    {
        if (typeof(TEntity) == typeof(Product))
        {
            var entity = await _dbContext.Set<Product>()
                .Include(a => a.Attributes)
                .Include(i => i.ProductImages)
                .FirstOrDefaultAsync(x => x.Id == Guid.Parse(id.ToString()));

            return entity as TEntity;
        }
        else
        {
            var entity = await _entity.FindAsync(id, cancellationToken);
            return entity;
        }
    }
}
