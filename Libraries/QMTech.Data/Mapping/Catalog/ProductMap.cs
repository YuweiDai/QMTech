using QMTech.Core.Domain.Catalog;

namespace QMTech.Data.Mapping.Catalog
{
    public partial class ProductMap : EntityTypeConfiguration<Product>
    {
        public ProductMap()
        {
            this.ToTable("Product");
            this.HasKey(p => p.Id);
            this.Property(p => p.Name).IsRequired().HasMaxLength(400);
            this.Property(p => p.MetaKeywords).HasMaxLength(400);
            this.Property(p => p.MetaTitle).HasMaxLength(400);
            this.Property(p => p.Sku).HasMaxLength(400);
            this.Property(p => p.ManufacturerPartNumber).HasMaxLength(400);

            this.Property(p => p.Price).HasPrecision(18, 4);
            this.Property(p => p.OldPrice).HasPrecision(18, 4);
            this.Property(p => p.ProductCost).HasPrecision(18, 4);
            this.Property(p => p.SpecialPrice).HasPrecision(18, 4);


            this.Property(p => p.AllowedQuantities).HasMaxLength(1000);

            this.Ignore(p => p.ProductType);
            this.Ignore(p => p.GiftCardType);
            this.Ignore(p => p.LowStockActivity);
            this.Ignore(p => p.ManageInventoryMethod);
            this.Ignore(p => p.RecurringCyclePeriod);
            this.Ignore(p => p.RentalPricePeriod);


            this.HasMany(p => p.ProductTags)
                .WithMany(pt => pt.Products)
                .Map(m => m.ToTable("Product_ProductTag_Mapping"));
        }
    }
}
