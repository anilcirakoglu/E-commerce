using E_Commerce.WebApi.Business.Models;

namespace E_Commerce.WebApi.Business
{
    public interface IAdminBO
    {
        List<AdminModel> GetAll();//customer and seller create
        Task<AdminModel> GetByID(int ID, bool tracking = true);
        Task<AdminModel> Create(AdminModel admin);

        Task UpdateAsync(AdminModel admin);
        Task<int> SaveAsync();
        Task RemoveAsync(int ID);
    }
}
