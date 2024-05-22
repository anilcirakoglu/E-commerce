using E_Commerce.WebApi.Business.Models;

namespace E_Commerce.WebApi.Business
{
    public interface ISellerBO
    {
        List<SellerDto> GetAll();
        Task<SellerDto> GetByID(int ID, bool tracking = true);
        Task<SellerDto> Create(SellerDto product);

        string Login(LoginModel loginModel);
        Task<SellerDto> Registration(SellerDto sellerDto);
        Task UpdateAsync(SellerDto seller);

        Task ActiveProduct(int ID);
        Task PassiveProduct(int ID);

        Task<int> SaveAsync();
        Task RemoveAsync(int ID);
    }
}
