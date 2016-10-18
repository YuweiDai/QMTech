using QMTech.Core.Domain.Authentication;

namespace QMTech.Data.Mapping.Authentication
{
    public class RefreshTokenMap : EntityTypeConfiguration<RefreshToken>
    {
        public RefreshTokenMap()
        {
            this.HasKey(tc => tc.Id);
        }
    }
}
