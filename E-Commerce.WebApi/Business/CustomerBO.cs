using E_Commerce.WebApi.Application.Admins;
using E_Commerce.WebApi.Application.Customers;
using E_Commerce.WebApi.Application.Sellers;
using E_Commerce.WebApi.Business.Enums;
using E_Commerce.WebApi.Business.Models;
using E_Commerce.WebApi.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.WebApi.Business
{
    public class CustomerBO : ICustomerBO
    {
        readonly private IAdminReadRepository _adminReadRepository;
        readonly private IAdminWriteRepository _adminWriteRepository;

        readonly private ICustomerReadRepository _customerReadRepository;
        readonly private ICustomerWriteRepository _customerWriteRepository;

        readonly private ISellerReadRepository _sellerReadRepository;
        readonly private ISellerWriteRepository _sellerWriteRepository;

        public CustomerBO(IAdminReadRepository adminReadRepository, IAdminWriteRepository adminWriteRepository, ICustomerReadRepository customerReadRepository, ICustomerWriteRepository customerWriteRepository, ISellerReadRepository sellerReadRepository, ISellerWriteRepository sellerWriteRepository)
        {
            _adminReadRepository = adminReadRepository;
            _adminWriteRepository = adminWriteRepository;
            _customerReadRepository = customerReadRepository;
            _customerWriteRepository = customerWriteRepository;
            _sellerReadRepository = sellerReadRepository;
            _sellerWriteRepository = sellerWriteRepository;
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
                Role = RoleType.Customer.ToString(),
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
                    Role = RoleType.Customer.ToString(),
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
                Role = RoleType.Customer.ToString(),
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
        #region RegisAndLogin
        public async Task<CustomerDto> Registration(CustomerDto customerDto)
        {
            var adminExists = await _adminReadRepository.GetWhere(x => x.Email == customerDto.Email).FirstOrDefaultAsync();
            var customerExists = await _customerReadRepository.GetWhere(x => x.Email == customerDto.Email).FirstOrDefaultAsync();
            var sellerExists = await _sellerReadRepository.GetWhere(x=>x.Email == customerDto.Email).FirstOrDefaultAsync();

            if (customerExists == null&&sellerExists==null && adminExists == null)
            {
                var customer = new Customer()
                {
                    FirstName = customerDto.FirstName,
                    LastName = customerDto.LastName,
                    Address = customerDto.Address,
                    Email = customerDto.Email,
                    Password = customerDto.Password,
                    PhoneNumber = customerDto.PhoneNumber,
                    Role = RoleType.Customer.ToString()
                };
                await _customerWriteRepository.AddAsync(customer);
                await _customerWriteRepository.SaveAsync();

            }
            return customerDto;
        }

        public string Login(LoginModel loginModel) 
        {
            var customer = _customerReadRepository.GetWhere(x=>x.Email == loginModel.Email).FirstOrDefault();
            var customerPassword = _customerReadRepository.GetWhere(x=>x.Password == loginModel.Password).FirstOrDefault();
            if (customer == null || customerPassword == null)
            {
                return "Invalid Email or Password";
            }

            else { return "okey"; }
                
            
        }
        #endregion
    }
}
