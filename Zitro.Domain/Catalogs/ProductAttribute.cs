namespace Zitro.Domain.Catalogs;

public class ProductAttribute : Entity<Guid>
{
    public ProductAttribute()
    {
        
    }
    protected ProductAttribute(Guid id) : base(id)
    {
    }

    public Guid ProductId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}
