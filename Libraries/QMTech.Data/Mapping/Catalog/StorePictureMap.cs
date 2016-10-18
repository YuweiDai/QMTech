using QMTech.Core.Domain.Catalog;

namespace QMTech.Data.Mapping.Catalog
{
    public class StorePictureMap : EntityTypeConfiguration<StorePicture>
    {
        public StorePictureMap()
        {
            this.ToTable("Store_Picture_Mapping");
            this.HasKey(pp => pp.Id);

            this.HasRequired(pp => pp.Picture)
                .WithMany()
                .HasForeignKey(pp => pp.PictureId);


            this.HasRequired(pp => pp.Store)
                .WithMany(p => p.StorePictures)
                .HasForeignKey(pp => pp.StoreId);
        }
    }
 
}
