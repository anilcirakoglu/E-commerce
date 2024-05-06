using E_Commerce.WebApi.Business.Models;

namespace E_Commerce.WebApi.Business
{
    public interface ISellerBO
    {
        List<SellerModel> GetAll();
        Task<SellerModel> GetByID(int ID, bool tracking = true);
        Task<SellerDto> Create(SellerDto product);

        string Login(LoginModel loginModel);
        Task<SellerDto> Registration(SellerDto sellerDto);
        Task UpdateAsync(SellerDto seller);
        
        Task<int> SaveAsync();
        Task RemoveAsync(int ID);
    }
}
