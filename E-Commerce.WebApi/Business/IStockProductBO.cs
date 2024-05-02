using E_Commerce.WebApi.Business.Models;

namespace E_Commerce.WebApi.Business
{
    public interface IStockProductBO
    {
        List<StockProductModel> GetAll();
        Task<StockProductModel> GetByID(int ID, bool tracking = true);
        Task<StockProductModel> Create(StockProductModel product);

        Task UpdateAsync(StockProductModel seller);
        Task<int> SaveAsync();
        Task RemoveAsync(int ID);
    }
}
