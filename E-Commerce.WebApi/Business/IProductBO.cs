using E_Commerce.WebApi.Business.Models;

namespace E_Commerce.WebApi.Business
{
    public interface IProductBO
    {
        List<ProductModel> GetAll();
        List<GetAllProductsForAdmin> GetAllProductsForAdmin();
        List<ProductDto> sellerProducts(int ID);

        Task<ProductModel> GetByID(int ID,bool tracking =true);
        Task<ProductDto> Create(ProductDto product);

        Task UpdateAsync(ProductModel product);
        Task<int> SaveAsync();
        Task RemoveAsync(int ID);
    }
}
