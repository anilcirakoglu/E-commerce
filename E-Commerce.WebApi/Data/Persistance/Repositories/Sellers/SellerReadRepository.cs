using E_Commerce.WebApi.Application.Sellers;
using E_Commerce.WebApi.Data.Entities;
using E_Commerce.WebApi.Data.Persistance.Context;

namespace E_Commerce.WebApi.Data.Persistance.Repositories.Sellers
{
    public class SellerReadRepository : ReadRepository<Seller>, ISellerReadRepository
    {
        public SellerReadRepository(AppDbContext context) : base(context)
        {
        }
    }
}
