using E_Commerce.WebApi.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_Commerce.WebApi.Data.Persistance.EntityConfigurations
{
    public class ProductConfig : IEntityTypeConfiguration<Product>
    {


        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Product");
            builder.HasKey(p => p.ID);
            builder.Property(p => p.ProductName).IsRequired().HasMaxLength(250);
            builder.Property(p => p.ProductInformation).IsRequired().HasMaxLength(800);
            builder.Property(p => p.IsProductActive);
            builder.Property(p => p.ProductPrice).IsRequired().HasMaxLength(250);
            builder.Property(p => p.previousPrice);
            builder.Property(p => p.discountPercentage);
            builder.Property(p => p.Image);
           
            builder.HasOne(p=>p.CategoryProduct).WithMany().HasForeignKey(c=>c.CategoryID).IsRequired();
            // builder.HasOne(p => p.CategoryProduct).WithOne().HasForeignKey<Product>(p => p.CategoryID).IsRequired();
            builder.HasOne(p => p.Seller).WithMany().HasForeignKey(c => c.SellerID).IsRequired();
            
        }
    } 
}