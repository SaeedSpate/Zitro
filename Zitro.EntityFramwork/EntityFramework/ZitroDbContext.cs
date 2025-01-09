using Microsoft.EntityFrameworkCore;
using Zitro.Domain;
using Zitro.Domain.Catalogs;

namespace Zitro.EntityFramwork.EntityFramework;

public class ZitroDbContext : DbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductImage> ProductImages { get; set; }
    public DbSet<ProductAttribute> ProductAttributes { get; set; }

    public ZitroDbContext(DbContextOptions<ZitroDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Model.GetEntityTypes()
            .Where(entityType => typeof(IEntity).IsAssignableFrom(entityType.ClrType))
            .ToList()
            .ForEach(entityType =>
            {
                var rowVersionProperty = entityType.FindProperty(nameof(IEntity.RowVersion));
                if (rowVersionProperty != null)
                {
                    rowVersionProperty.IsConcurrencyToken = true;
                    rowVersionProperty.ValueGenerated = Microsoft.EntityFrameworkCore.Metadata.ValueGenerated.OnAddOrUpdate;
                }
            });

        modelBuilder.Entity<Product>()
            .HasMany(p => p.ProductImages)
            .WithOne()
            .HasForeignKey(p => p.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Product>()
            .HasMany(p => p.Attributes)
            .WithOne()
            .HasForeignKey(p => p.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Product>()
            .Property(p => p.RowVersion)
            .IsRowVersion();

        modelBuilder.Entity<ProductImage>()
            .Property(p => p.RowVersion)
            .IsRowVersion();

        modelBuilder.Entity<ProductAttribute>()
            .Property(p => p.RowVersion)
            .IsRowVersion();
    }
}
