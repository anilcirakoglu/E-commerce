using E_Commerce.WebApi.Application.CategoryProducts;
using E_Commerce.WebApi.Data.Entities;
using E_Commerce.WebApi.Data.Persistance.Context;

namespace E_Commerce.WebApi.Data.Persistance.Repositories.CategoryProducts
{
    public class CategoryProductWriteRepository : WriteRepository<CategoryProduct>, ICategoryProductWriteRepository
    {
        public CategoryProductWriteRepository(AppDbContext context) : base(context)
        {
        }
    }
}
