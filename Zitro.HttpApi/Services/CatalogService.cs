using Zitro.Domain.Catalogs;
using Zitro.EntityFramwork.UnitOfWorks;

namespace Zitro.HttpApi.Services;

public class CatalogService : ICatalogService
{

    private readonly IUnitOfWork<Product, Guid> _unitOfWork;
    private readonly IUnitOfWork<ProductAttribute, Guid> _productAttributeUow;
    private readonly IUnitOfWork<ProductImage, Guid> _productImageUow;
    public CatalogService(
        IUnitOfWork<Product, Guid> unitOfWork,
        IUnitOfWork<ProductAttribute, Guid> productAttributeUow,
        IUnitOfWork<ProductImage, Guid> productImageUow)
    {
        _unitOfWork = unitOfWork;
        _productAttributeUow = productAttributeUow;
        _productImageUow = productImageUow;
    }

    public async Task<Product> AddProductAsync(Product product)
    {
        var result = await _unitOfWork.Repository.InsertAsync(product, true); 
        return result;
    }

    public async Task DeleteProductAsync(Guid id)
    {
        await _unitOfWork.Repository.DeleteAsync(id, true);
    }

    public async Task<Product> GetProductAsync(Guid id)
    {
        var result = await _unitOfWork.Repository.GetAsync(id);
        return result;
    }

    public async Task<List<Product>> GetProductListAsync()
    {
        var result = await _unitOfWork.Repository.GetListAsync();
        return result;
    }

    public async Task<Product> UpdateProductAsync(Product product)
    {
        var result = await _unitOfWork.Repository.UpdateAsync(product, true);
        return result;
    }

    public async Task DeleteProductAttributeByIdAsync(Guid id)
    {
        await _productAttributeUow.Repository.DeleteAsync(id, true);
    }

    public async Task DeleteProductImageByIdAsync(Guid id)
    {
        await _productImageUow.Repository.DeleteAsync(id, true);
    }
}
