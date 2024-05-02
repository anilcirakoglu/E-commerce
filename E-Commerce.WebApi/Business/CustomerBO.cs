using E_Commerce.WebApi.Application.Customers;
using E_Commerce.WebApi.Business.Models;
using E_Commerce.WebApi.Data.Entities;

namespace E_Commerce.WebApi.Business
{
    public class CustomerBO : ICustomerBO
    {
        readonly private ICustomerReadRepository _customerReadRepository;
        readonly private ICustomerWriteRepository _customerWriteRepository;
        public CustomerBO(ICustomerReadRepository customerReadRepository, ICustomerWriteRepository customerWriteRepository)
        {
            _customerReadRepository = customerReadRepository;
            _customerWriteRepository = customerWriteRepository;
        }
        public async Task<CustomerModel> Create(CustomerModel customerModel)
        {
            var customer = new Customer()
            {
                ID = customerModel.ID,
                FirstName = customerModel.FirstName,
                LastName = customerModel.LastName,
                Address = customerModel.Address,
                Email = customerModel.Email,
                Password = customerModel.Password,
                PhoneNumber = customerModel.PhoneNumber,
                Role = customerModel.Role,
            };
            await _customerWriteRepository.AddAsync(customer);
            await _customerWriteRepository.SaveAsync();
            return customerModel;
        }

        public List<CustomerModel> GetAll()
        {
            var customers = _customerReadRepository.GetAll().ToList();
            var customerList = new List<CustomerModel>();
            foreach (var customer in customers)
            {
                var cstlist = new CustomerModel()
                {
                    ID = customer.ID,
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    Address = customer.Address,
                    Email = customer.Email,
                    Password = customer.Password,
                    PhoneNumber = customer.PhoneNumber,
                    Role = customer.Role,
                };
                customerList.Add(cstlist);
            }
            return customerList;
        }

        public async Task<CustomerModel> GetByID(int ID, bool tracking = true)
        {
            var customers = await _customerReadRepository.GetByIDAsync(ID);
            var customer = new CustomerModel()
            {
                ID = customers.ID,
                FirstName = customers.FirstName,
                LastName = customers.LastName,
                Address = customers.Address,
                Email = customers.Email,
                Password = customers.Password,
                PhoneNumber = customers.PhoneNumber,
                Role = customers.Role,
            };
            return customer;
        }

        public async Task RemoveAsync(int ID)
        {
            var customer = await _customerReadRepository.GetByIDAsync(ID);
            var customerRemove = new CustomerModel()
            {
                ID = customer.ID,
                FirstName= customer.FirstName,
                LastName = customer.LastName,
                Address = customer.Address,
                Email = customer.Email,
                Password= customer.Password,
                PhoneNumber = customer.PhoneNumber,
                Role = customer.Role,
            };
            await _customerWriteRepository.RemoveAsync(ID);
            await _customerWriteRepository.SaveAsync();
        }

        public async Task<int> SaveAsync()
        {
            var customer = await _customerWriteRepository.SaveAsync();
            return customer;
        }

        public async Task UpdateAsync(CustomerModel customer)
        {
           var customers =_customerReadRepository.GetAll().FirstOrDefault(x=>x.ID == customer.ID);

            customers.FirstName= customer.FirstName;
            customers.LastName= customer.LastName;
            customers.Address= customer.Address;
            customers.Email= customer.Email;
            customers.Password= customer.Password;
            customers.PhoneNumber= customer.PhoneNumber;
            customers.Role= customer.Role;//kaldırılacak role kendi değiştiremez
            _customerWriteRepository.Update(customers);
            await _customerWriteRepository.SaveAsync();
        }
    }
}
