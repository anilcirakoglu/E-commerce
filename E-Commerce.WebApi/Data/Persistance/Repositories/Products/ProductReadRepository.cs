using E_Commerce.WebApi.Application.Products;
using E_Commerce.WebApi.Data.Entities;
using E_Commerce.WebApi.Data.Persistance.Context;

namespace E_Commerce.WebApi.Data.Persistance.Repositories
{
    public class ProductReadRepository:ReadRepository<Product>,IProductReadRepository
    {
        public ProductReadRepository(AppDbContext context):base(context) 
        {
            
        }
    }
}
