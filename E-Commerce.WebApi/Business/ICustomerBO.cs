using E_Commerce.WebApi.Business.Models;

namespace E_Commerce.WebApi.Business
{
    public interface ICustomerBO
    {
        List<CustomerModel> GetAll();//customer and seller create
        Task<CustomerModel> GetByID(int ID, bool tracking = true);
        Task<CustomerModel> Create(CustomerModel product);

        string Login(LoginModel loginModel);
        Task<CustomerDto>Registration(CustomerDto customerDto);

        Task UpdateAsync(CustomerDto customer);
        Task<int> SaveAsync();
        Task RemoveAsync(int ID);
    }
}
