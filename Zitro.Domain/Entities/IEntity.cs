using System.ComponentModel.DataAnnotations;

namespace Zitro.Domain;

public interface IEntity 
{
    [Timestamp]
    public byte[] RowVersion { get; set; }
}
public interface IEntity<TKey>
{
    TKey Id { get; }
}
