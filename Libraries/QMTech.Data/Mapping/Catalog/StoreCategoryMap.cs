using QMTech.Core.Domain.Catalog;

namespace QMTech.Data.Mapping.Catalog
{
    public partial class StoreCategoryMap : EntityTypeConfiguration<StoreCategory>
    {
        public StoreCategoryMap()
        {
            this.ToTable("StoreCategory");
            this.HasKey(c => c.Id);
            this.Property(c => c.Name).IsRequired().HasMaxLength(400);
            this.Property(c => c.MetaKeywords).HasMaxLength(400);
            this.Property(c => c.MetaTitle).HasMaxLength(400);

            this.HasMany(pc => pc.Stores)
                .WithRequired(s => s.StoreCategory);

        }
    }
}
