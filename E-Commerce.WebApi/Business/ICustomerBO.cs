using E_Commerce.WebApi.Business.Models;

namespace E_Commerce.WebApi.Business
{
    public interface ICustomerBO
    {
        List<CustomerModel> GetAll();//customer and seller create
        Task<CustomerModel> GetByID(int ID, bool tracking = true);
        Task<CustomerModel> Create(CustomerModel product);

        Task UpdateAsync(CustomerModel product);
        Task<int> SaveAsync();
        Task RemoveAsync(int ID);
    }
}
