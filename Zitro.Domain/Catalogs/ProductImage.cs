namespace Zitro.Domain.Catalogs;

public class ProductImage : Entity<Guid>
{
    public ProductImage()
    {
        
    }
    protected ProductImage(Guid id) : base(id)
    {
    }

    public string ImageName { get; set; }
    public string ImageUrl { get; set; }
    public bool IsCoverImage { get; set; } = false;
    public Guid ProductId { get; set; }
}
