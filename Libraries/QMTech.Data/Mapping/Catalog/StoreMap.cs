using QMTech.Core.Domain.Catalog;

namespace QMTech.Data.Mapping.Catalog
{
    public class StoreMap:EntityTypeConfiguration<Store>
    {
        public StoreMap()
        {
            this.ToTable("Store");
            this.HasKey(s => s.Id);
            this.Property(s => s.Name).IsRequired().HasMaxLength(400);
            this.Property(s => s.Address).HasMaxLength(400);
            this.Property(s => s.MetaKeywords).HasMaxLength(400);
            this.Property(s => s.MetaTitle).HasMaxLength(400);

            this.HasMany(s => s.Products)
                .WithRequired(p => p.Store);
        }
    }
}
