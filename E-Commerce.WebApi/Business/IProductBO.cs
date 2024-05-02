using E_Commerce.WebApi.Business.Models;

namespace E_Commerce.WebApi.Business
{
    public interface IProductBO
    {
        List<ProductModel> GetAll();
        Task<ProductModel> GetByID(int ID,bool tracking =true);
        Task<ProductModel> Create(ProductModel product);

        Task UpdateAsync(ProductModel product);
        Task<int> SaveAsync();
        Task RemoveAsync(int ID);
    }
}
