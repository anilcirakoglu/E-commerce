using E_Commerce.WebApi.Application.Carts;
using E_Commerce.WebApi.Data.Entities;
using E_Commerce.WebApi.Data.Persistance.Context;

namespace E_Commerce.WebApi.Data.Persistance.Repositories.Carts
{
    public class CartWriteRepository:WriteRepository<Cart>,ICartWriteRepository
    {
        public CartWriteRepository(AppDbContext context):base(context) 
        {
        }
    }
}
