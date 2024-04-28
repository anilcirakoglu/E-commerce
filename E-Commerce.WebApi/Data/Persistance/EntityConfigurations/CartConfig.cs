using E_Commerce.WebApi.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_Commerce.WebApi.Data.Persistance.EntityConfigurations
   
{
    public class CartConfig : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            builder.ToTable("Cart");
            builder.HasKey(p => p.ID);
            builder.HasOne(p=>p.Product).WithMany().HasForeignKey(c=>c.ProductID).IsRequired();
            builder.HasOne(p => p.Customer).WithMany().HasForeignKey(c => c.CustomerID).IsRequired();

        }
    }
}
