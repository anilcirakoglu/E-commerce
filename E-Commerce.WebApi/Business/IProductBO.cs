using E_Commerce.WebApi.Business.Models;

namespace E_Commerce.WebApi.Business
{
    public interface IProductBO
    {
        List<ProductModel> GetAll();
        List<AllProducts> GetAllProductsForAdmin();
        List<AllProducts> GetAllProducts();
        List<ProductDto> sellerProducts(int ID);
        Task<ProductDetailForCustomer> DetailForCustomer(int ID);
        Task<ProductModel> GetByID(int ID,bool tracking =true);
        Task<ProductDto> Create(ProductDto product);

        List<AllProducts> Search(string name);

        Task UpdateAsync(ProductModel product);
        Task<int> SaveAsync();
        Task RemoveAsync(int ID);
    }
}
