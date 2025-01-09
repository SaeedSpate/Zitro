namespace Zitro.Domain;

[Serializable]
public abstract class Entity : IEntity
{
    protected Entity()
    {
        
    }
    public virtual byte[] RowVersion { get; set; } = BitConverter.GetBytes(DateTime.UtcNow.Ticks);
}

[Serializable]
public abstract class Entity<TKey> : 
    Entity, 
    IEntity<TKey>
{
    protected Entity() { }

    protected Entity(TKey id)
    {
        Id = id;
    }

    public virtual TKey Id { get; protected set; } = default!;
}
