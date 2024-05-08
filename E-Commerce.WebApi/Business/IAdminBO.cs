using E_Commerce.WebApi.Business.Models;

namespace E_Commerce.WebApi.Business
{
    public interface IAdminBO
    {
        List<AdminDto> GetAll();//customer and seller create
        Task<AdminDto> GetByID(int ID, bool tracking = true);
        
        string Login(LoginModel loginModel);
        Task ApprovedSeller(int ID);
        Task RejectSeller(int ID);
        Task<AdminDto> Registration(AdminDto customerDto);
        Task UpdateAsync(AdminDto admin);
       
        Task<int> SaveAsync();
        Task RemoveAsync(int ID);
    }
}
