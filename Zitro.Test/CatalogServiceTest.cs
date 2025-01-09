using Microsoft.EntityFrameworkCore;
using Zitro.Domain.Catalogs;
using Zitro.EntityFramwork.EntityFramework;
using Zitro.EntityFramwork.UnitOfWorks;
using Zitro.HttpApi.Services;

namespace Zitro.Test
{
    public class CatalogServiceTest
    {

        [Fact]
        public async Task AddProduct_ShouldAddProductToDatabase()
        {
            using ZitroDbContext dbContext = CreateDbContextInstance();
            var catalogService = CreateCatalogServiceInstance(dbContext);

            Product product = CreateProduct("Test Product", "Test Description", 150);

            await catalogService.AddProductAsync(product);

            Assert.Single(dbContext.Products);
            Assert.Equal("Test Product", dbContext.Products.First().Name);
        }

        [Fact]
        public async Task GetAllProduct_ShouldReturnAllProducts()
        {
            using ZitroDbContext dbContext = CreateDbContextInstance();
            var catalogService = CreateCatalogServiceInstance(dbContext);

            dbContext.Products.AddRange(
                CreateProduct("Test Product 01", "Test Description 01", 150),
                CreateProduct("Test Product 02", "Test Description 02", 300));
            dbContext.SaveChanges();

            var products = await catalogService.GetProductListAsync();

            Assert.Equal(2, products.Count);
        }

        [Fact]
        public async Task GetProductById_ShouldBeGetProductById()
        {
            using ZitroDbContext dbContext = CreateDbContextInstance();
            var catalogService = CreateCatalogServiceInstance(dbContext);

            dbContext.Products.Add(CreateProduct("Test Product 01", "Test Description 01", 150));
            dbContext.SaveChanges();

            var product = await catalogService.GetProductAsync(dbContext.Products.First().Id);

            Assert.Equal("Test Product 01", product.Name);
        }

        [Fact]
        public async Task DeleteProduct_ShouldDeletProduct()
        {
            using ZitroDbContext dbContext = CreateDbContextInstance();
            var catalogService = CreateCatalogServiceInstance(dbContext);

            dbContext.Products.Add(CreateProduct("Test Product", "Test Description", 150));
            dbContext.SaveChanges();
            var product = dbContext.Products.First();
            await catalogService.DeleteProductAsync(product.Id);

            Assert.Empty(dbContext.Products);
        }
        
        [Fact]
        public async Task UpdateProduct_ShouldUpdateProduct()
        {
            using ZitroDbContext dbContext = CreateDbContextInstance();
            var catalogService = CreateCatalogServiceInstance(dbContext);

            var product = CreateProduct("Test Product 01", "Test Description 01", 150);
            dbContext.Products.Add(product);
            dbContext.SaveChanges();

            product.Name = "Updated Test Product";
            await catalogService.UpdateProductAsync(product);

            Assert.Equal("Updated Test Product", product.Name);
        }

        [Fact]
        public async Task DeleteProductAttributeById_ShouldDeleteProductAttributeById()
        {
            using ZitroDbContext dbContext = CreateDbContextInstance();
            var catalogService = CreateCatalogServiceInstance(dbContext);

            dbContext.Products.Add(CreateProduct("Test Product", "Test Description", 150));
            dbContext.SaveChanges();
            var product = dbContext.Products.First();
            await catalogService.DeleteProductAttributeByIdAsync(product.Attributes.First().Id);

            Assert.Empty(product.Attributes);
        }


        [Fact]
        public async Task DeleteProductImageById_ShouldDeleteProductImageById()
        {
            using ZitroDbContext dbContext = CreateDbContextInstance();
            var catalogService = CreateCatalogServiceInstance(dbContext);

            dbContext.Products.Add(CreateProduct("Test Product", "Test Description", 150));
            dbContext.SaveChanges();
            var product = dbContext.Products.First();
            await catalogService.DeleteProductImageByIdAsync(product.ProductImages.First().Id);

            Assert.Empty(product.ProductImages);
        }

        private static DbContextOptions<ZitroDbContext> GetInMemoryDbContextOptions()
        {
            return new DbContextOptionsBuilder<ZitroDbContext>()
                .UseInMemoryDatabase($"ZitroTestDb_{Guid.NewGuid()}")
                .Options;
        }

        private static ZitroDbContext CreateDbContextInstance()
        {
            var options = GetInMemoryDbContextOptions();
            var dbContext = new ZitroDbContext(options);
            return dbContext;
        }

        private static CatalogService CreateCatalogServiceInstance(ZitroDbContext dbContext)
        {
            var uowProduct = new UnitOfWork<Product, Guid>(dbContext);
            var uowProductAttribute = new UnitOfWork<ProductAttribute, Guid>(dbContext);
            var uowImage = new UnitOfWork<ProductImage, Guid>(dbContext);

            return new CatalogService(uowProduct, uowProductAttribute, uowImage);
        }

        private static Product CreateProduct(string productName, string description, decimal price)
        {
            var productImage = new ProductImage
            {
                ImageName = "Test",
                ImageUrl = "Test",
                IsCoverImage = true
            };

            var productImages = new List<ProductImage>();
            productImages.Add(productImage);

            var productAttribute = new ProductAttribute
            {
                Name = "Test",
                Description = "Test",
            };

            var attributes = new List<ProductAttribute>();
            attributes.Add(productAttribute);

            var product = new Product
            {
                Name = productName,
                Description = description,
                Price = price,
                ProductImages = productImages,
                Attributes = attributes
            };
            return product;
        }

    }
}
