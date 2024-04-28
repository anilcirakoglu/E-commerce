using E_Commerce.WebApi.Application.StockProducts;
using E_Commerce.WebApi.Data.Entities;
using E_Commerce.WebApi.Data.Persistance.Context;

namespace E_Commerce.WebApi.Data.Persistance.Repositories.StockProducts
{
    public class StockProductWriteRepository : WriteRepository<StockProduct>, IStockProductWriteRepository
    {
        public StockProductWriteRepository(AppDbContext context) : base(context)
        {
        }
    }
}
