using E_Commerce.WebApi.Application.StockProducts;
using E_Commerce.WebApi.Data.Entities;
using E_Commerce.WebApi.Data.Persistance.Context;

namespace E_Commerce.WebApi.Data.Persistance.Repositories.StockProducts
{
    public class StockProductReadRepository : ReadRepository<StockProduct>, IStockProductReadRepository
    {
        public StockProductReadRepository(AppDbContext context) : base(context)
        {
        }
    }
}
