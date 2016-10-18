using QMTech.Core.Domain.Identity;

namespace QMTech.Data.Mapping.Identity
{
    public class ApplicationUserMap:EntityTypeConfiguration<ApplicationUser>
    {
        public ApplicationUserMap()
        {
            this.ToTable("ApplicationUser");
        }
    }
}
