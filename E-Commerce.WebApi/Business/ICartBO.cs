using E_Commerce.WebApi.Business.Models;

namespace E_Commerce.WebApi.Business
{
    public interface ICartBO
    {
        List<CartModel> GetAll();
        Task<CartModel> GetByID(int ID, bool tracking = true);
        Task<CartModel> Create(CartModel cart);
      

        Task UpdateAsync(CartModel cart);
        Task<int> SaveAsync();
        Task RemoveAsync(int ID);
    }
}
