using Microsoft.AspNetCore.Mvc;
using Zitro.Domain.Catalogs;
using Zitro.HttpApi.Services;

namespace Zitro.HttpApi.Controllers;

[ApiController]
[Route("/api/catalog")]
public class CatalogController : Controller
{
    private readonly ICatalogService _catalogService;

    public CatalogController(ICatalogService catalogService)
    {
        _catalogService = catalogService;
    }

    [HttpGet("GetProductById/{id}")]
    public async Task<IActionResult> GetProductAsync(Guid id)
    {
        var product = await _catalogService.GetProductAsync(id);
        
        if (product == null)
            return BadRequest("Product not found!");
        
        return Ok(product);
    }

    [HttpGet("GetProductList")]
    public async Task<IActionResult> GetProductListAsync()
    {
        var products = await _catalogService.GetProductListAsync();

        if (products.Count <= 0)
            return BadRequest("No product found in the catalog!");

        return Ok(products);
    }

    [HttpPost("AddProduct")]
    public async Task<IActionResult> AddProduct(Product product)
    {
        if (product == null)
            return BadRequest("Product must be not null!");

        var newProduct = await _catalogService.AddProductAsync(product);
        return Ok(newProduct);
    }

    [HttpPost("UpdateProduct")]
    public async Task<IActionResult> UpdateProductAsync(Product product)
    {
        if (product == null)
            return BadRequest("Product must be not null!");

        var updatedtProduct = await _catalogService.UpdateProductAsync(product);
        return Ok(updatedtProduct);
    }

    [HttpDelete("DeleteProductById")]
    public async Task<IActionResult> DeleteProductByIdAsync(Guid id)
    {
        var product = await _catalogService.GetProductAsync(id);

        if (product == null)
            return BadRequest("Product not found");

        await _catalogService.DeleteProductAsync(id);

        return Ok($"Product with ID: {id}, removed");
    }

    [HttpDelete("DeleteProductAttributeById")]
    public async Task<IActionResult> DeleteProductAttributeByIdAsync(Guid id)
    {
        try
        {
            await _catalogService.DeleteProductAttributeByIdAsync(id);

            return Ok($"Product Attribute with ID: {id}, removed");

        }
        catch (Exception)
        {
            return BadRequest("Product Attribute not found");
        }
    }

    [HttpDelete("DeleteProductImageById")]
    public async Task<IActionResult> DeleteProductImageByIdAsync(Guid id)
    {
        try
        {
            await _catalogService.DeleteProductImageByIdAsync(id);

            return Ok($"Product Image with ID: {id}, removed");

        }
        catch (Exception)
        {
            return BadRequest("Product Image not found");
        }
    }
}
