using E_Commerce.WebApi.Application.Admins;
using E_Commerce.WebApi.Application.Carts;
using E_Commerce.WebApi.Application.Customers;
using E_Commerce.WebApi.Application.Sellers;
using E_Commerce.WebApi.Business.Enums;
using E_Commerce.WebApi.Business.Models;
using E_Commerce.WebApi.Business.Token;
using E_Commerce.WebApi.Data.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

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

        readonly private ICartReadRepository _cartReadRepository;
        readonly private ICartWriteRepository _cartWriteRepository;

        readonly private IConfiguration _configuration;

        public CustomerBO(IAdminReadRepository adminReadRepository, IAdminWriteRepository adminWriteRepository, ICustomerReadRepository customerReadRepository, ICustomerWriteRepository customerWriteRepository, ISellerReadRepository sellerReadRepository, ISellerWriteRepository sellerWriteRepository, ICartReadRepository cartReadRepository, ICartWriteRepository cartWriteRepository, IConfiguration configuration)
        {
            _adminReadRepository = adminReadRepository;
            _adminWriteRepository = adminWriteRepository;
            _customerReadRepository = customerReadRepository;
            _customerWriteRepository = customerWriteRepository;
            _sellerReadRepository = sellerReadRepository;
            _sellerWriteRepository = sellerWriteRepository;
            _cartReadRepository = cartReadRepository;
            _cartWriteRepository = cartWriteRepository;
            _configuration = configuration;
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

        public async Task UpdateAsync(CustomerDto customer)
        {
          
            var customers = _customerReadRepository.GetAll().FirstOrDefault(x => x.ID == customer.ID);

            customers.FirstName = customer.FirstName;
            customers.LastName = customer.LastName;
            customers.Address = customer.Address;
            customers.Email = customer.Email;
            customers.Password = customer.Password;
            customers.PhoneNumber = customer.PhoneNumber;
          
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
           
            var customer = _customerReadRepository.GetWhere(x=>x.Email == loginModel.Email &&x.Password == loginModel.Password).FirstOrDefault();
           
            if (customer == null )
            {
                return "";

            }
            var tokenclaims = new List<Claim>
            {
               new Claim(ClaimTypes.NameIdentifier,customer.ID.ToString()),
               new Claim(ClaimTypes.Role,customer.Role,RoleType.Customer.ToString()),
               new Claim(ClaimTypes.Name,customer.Email),
               new Claim(ClaimTypes.Name,customer.Password),
               new Claim(ClaimTypes.Name,customer.ID.ToString())
           };

            var token = GenerateTokens(tokenclaims);
           
            return token;


        }
        public string GenerateTokens(IEnumerable<Claim> claims)
        {
            var autSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:KEY"]));
            if (autSigningKey.KeySize < 128)
            {
                using var hmac = new HMACSHA256();
                autSigningKey = new SymmetricSecurityKey(hmac.Key);
            }
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Issuer"],
                Expires = DateTime.UtcNow.AddHours(3),
                SigningCredentials = new SigningCredentials(autSigningKey, SecurityAlgorithms.HmacSha256),
                Subject = new ClaimsIdentity(claims),
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        #endregion

        public async Task AddProductCart(CartDto cartDto) 
        {
            var cart = new Cart()
            {
                ProductID = cartDto.ProductID,
                CustomerID = cartDto.CustomerID,
            };
            await _cartWriteRepository.AddAsync(cart);
            await _cartWriteRepository.SaveAsync();
        }


    }

    




}
