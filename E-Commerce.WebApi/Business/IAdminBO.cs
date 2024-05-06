using E_Commerce.WebApi.Business.Models;

namespace E_Commerce.WebApi.Business
{
    public interface IAdminBO
    {
        List<AdminModel> GetAll();//customer and seller create
        Task<AdminModel> GetByID(int ID, bool tracking = true);
        //Task<AdminModel> Create(AdminModel admin);
        string Login(LoginModel loginModel);
        Task ApprovedSeller(int ID);
        Task<AdminDto> Registration(AdminDto customerDto);
        Task UpdateAsync(AdminDto admin);
       
        Task<int> SaveAsync();
        Task RemoveAsync(int ID);
    }
}
