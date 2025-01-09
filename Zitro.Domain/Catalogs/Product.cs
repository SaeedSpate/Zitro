namespace Zitro.Domain.Catalogs;

public class Product : Entity<Guid>
{
    private decimal price;

    public Product()
    {
        
    }
    protected Product(Guid id) : base(id)
    {

    }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price 
    {
        get => price;
        set
        {
            if (value <= 0)
                throw new Exception("Price must be greater than 0.00");
            price = value;
        }
    }
    public List<ProductImage> ProductImages { get; set; } = new();
    public List<ProductAttribute> Attributes { get; set; } = new();
}
