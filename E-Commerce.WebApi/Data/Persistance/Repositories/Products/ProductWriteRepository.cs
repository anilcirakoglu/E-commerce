using E_Commerce.WebApi.Application.Products;
using E_Commerce.WebApi.Data.Entities;
using E_Commerce.WebApi.Data.Persistance.Context;

namespace E_Commerce.WebApi.Data.Persistance.Repositories.Products
{
    public class ProductWriteRepository: WriteRepository<Product>,IProductWriteRepository
    {
        public ProductWriteRepository(AppDbContext context):base(context)
        {
        }
    }
}
