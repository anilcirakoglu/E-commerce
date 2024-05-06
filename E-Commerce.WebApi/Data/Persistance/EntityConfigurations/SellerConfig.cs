using E_Commerce.WebApi.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_Commerce.WebApi.Data.Persistance.EntityConfigurations
{
    public class SellerConfig : IEntityTypeConfiguration<Seller>
    {
        public void Configure(EntityTypeBuilder<Seller> builder)
        {
            builder.ToTable("Seller");
            builder.HasKey(p => p.ID);
            builder.Property(p => p.FirstName).IsRequired().HasMaxLength(250);
            builder.Property(p => p.LastName).IsRequired().HasMaxLength(250);
            builder.Property(p => p.Address).IsRequired().HasMaxLength(250);
            builder.Property(p => p.PhoneNumber).IsRequired();
            builder.Property(p => p.Email).IsRequired();
            builder.Property(p => p.Password).IsRequired();
            builder.Property(p => p.CompanyType).IsRequired().HasMaxLength(250);
            builder.Property(p => p.TaxpayerIDNumber).IsRequired().HasMaxLength(250);
            builder.Property(p => p.Role).IsRequired().HasMaxLength(250);
            builder.Property(p => p.IsApprove);
        }
    }
}
