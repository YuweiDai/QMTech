using QMTech.Core.Domain.Catalog;

namespace QMTech.Data.Mapping.Catalog
{
    public partial class StoreTagMap : EntityTypeConfiguration<StoreTag>
    {
        public StoreTagMap()
        {
            this.ToTable("Store_Tags");
            this.HasKey(pt => pt.Id);

 
            this.HasRequired(pt => pt.Store).WithMany().HasForeignKey(pt => pt.StoreId);
        }
    }
}
