using E_Commerce.WebApi.Business.Models;

namespace E_Commerce.WebApi.Business
{
    public interface ICategoryProductBO
    {
        List<CategoryProductModel> GetAll();
        Task<CategoryProductModel> GetByID(int ID, bool tracking = true);
        Task<CategoryProductModel> Create(CategoryProductModel product);

        Task UpdateAsync(CategoryProductModel categoryProduct);

        List<AllProducts> CategoryList(int categoryID);
        Task<int> SaveAsync();
        Task RemoveAsync(int ID);
    }
}
