using Microsoft.EntityFrameworkCore;
using Zitro.Domain.Catalogs;
using Zitro.EntityFramwork.EntityFramework;
using Zitro.EntityFramwork.UnitOfWorks;
using Zitro.HttpApi.Services;

var builder = WebApplication.CreateBuilder(args);
var configurations = builder.Configuration;

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen(x =>
{
    x.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Zitro Catalog API", Version = "v1" });
});

builder.Services.AddDbContext<ZitroDbContext>(options =>
{
    options.UseSqlServer(configurations.GetConnectionString("Zitro"));
});

builder.Services.AddScoped(typeof(IUnitOfWork<Product, Guid>), typeof(UnitOfWork<Product, Guid>));
builder.Services.AddScoped(typeof(IUnitOfWork<ProductAttribute, Guid>), typeof(UnitOfWork<ProductAttribute, Guid>));
builder.Services.AddScoped(typeof(IUnitOfWork<ProductImage, Guid>), typeof(UnitOfWork<ProductImage, Guid>));
builder.Services.AddScoped(typeof(ICatalogService), typeof(CatalogService));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI(x =>
{
    x.SwaggerEndpoint("/swagger/v1/swagger.json", "Zitro API v1");
});

app.MapControllers();

app.Run();
