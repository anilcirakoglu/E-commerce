using E_Commerce.WebApi.Business.Models;

namespace E_Commerce.WebApi.Business
{
    public class StockProductBO : IStockProductBO
    {
        public Task<StockProductModel> Create(StockProductModel product)
        {
            throw new NotImplementedException();
        }

        public List<StockProductModel> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<StockProductModel> GetByID(int ID, bool tracking = true)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(int ID)
        {
            throw new NotImplementedException();
        }

        public Task<int> SaveAsync()
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(StockProductModel seller)
        {
            throw new NotImplementedException();
        }
    }
}
