using E_Commerce.WebApi.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace E_Commerce.WebApi.Data.Persistance.EntityConfigurations
{
    public class CategoryProductConfig : IEntityTypeConfiguration<CategoryProduct>
    {
        public void Configure(EntityTypeBuilder<CategoryProduct> builder)
        {

            builder.ToTable("CategoryProduct");
            builder.HasKey(p => p.ID);
            builder.Property(p => p.CategoryName).IsRequired().HasMaxLength(250);

        }
    }
}
