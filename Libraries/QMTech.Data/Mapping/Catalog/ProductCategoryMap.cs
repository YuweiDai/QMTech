using QMTech.Core.Domain.Catalog;

namespace QMTech.Data.Mapping.Catalog
{
    public partial class ProductCategoryMap : EntityTypeConfiguration<ProductCategory>
    {
        public ProductCategoryMap()
        {
            this.ToTable("ProductCategory");
            this.HasKey(pc => pc.Id);
            this.Property(pc => pc.Name).IsRequired().HasMaxLength(400);
            this.Property(pc => pc.MetaKeywords).HasMaxLength(400);
            this.Property(pc => pc.MetaTitle).HasMaxLength(400);

            this.HasMany(pc => pc.Products)
                 .WithRequired(p => p.ProductCategory);
        }
    }
}
