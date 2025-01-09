using Zitro.Domain.Catalogs;

namespace Zitro.HttpApi.Services;

public interface ICatalogService 
{
    Task<Product> GetProductAsync(Guid id);
    Task<List<Product>> GetProductListAsync();
    Task<Product> AddProductAsync(Product product);
    Task<Product> UpdateProductAsync(Product product);
    Task DeleteProductAsync(Guid id);
    Task DeleteProductAttributeByIdAsync(Guid id);
    Task DeleteProductImageByIdAsync(Guid id);
}
