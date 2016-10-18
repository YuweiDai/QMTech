using QMTech.Core.Domain.Customers;

namespace QMTech.Data.Mapping.Customers
{
    public partial class CustomerMap : EntityTypeConfiguration<Customer>
    {
        public CustomerMap()
        {
            this.ToTable("Customer");
            this.HasKey(c => c.Id);
            this.Property(u => u.UserName).HasMaxLength(1000);
            this.Property(u => u.Email).HasMaxLength(1000);
            this.Property(u => u.SystemName).HasMaxLength(400);

            this.Ignore(u => u.PasswordFormat);

            this.HasMany(c => c.CustomerRoles)
                .WithMany()
                .Map(m => m.ToTable("Customer_CustomerRole_Mapping"));

            this.HasMany(c => c.Addresses)
                .WithMany()
                .Map(m => m.ToTable("CustomerAddresses"));
        }
    }
}
