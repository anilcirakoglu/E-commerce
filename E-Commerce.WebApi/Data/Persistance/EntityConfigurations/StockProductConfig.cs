using E_Commerce.WebApi.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace E_Commerce.WebApi.Data.Persistance.EntityConfigurations
{
    public class StockProductConfig : IEntityTypeConfiguration<StockProduct>
    {
        public void Configure(EntityTypeBuilder<StockProduct> builder)
        {
            builder.ToTable("StockProduct");
            builder.HasKey(p => p.ID);
            builder.Property(p=>p.ProductQuantity).IsRequired();
            builder.HasOne(p => p.Product).WithMany().HasForeignKey(c => c.ProductID).IsRequired();
        }
    }
}
