using E_Commerce.WebApi.Application.Admins;
using E_Commerce.WebApi.Application.Customers;
using E_Commerce.WebApi.Application.Products;
using E_Commerce.WebApi.Application.Sellers;
using E_Commerce.WebApi.Business.Enums;
using E_Commerce.WebApi.Business.Models;
using E_Commerce.WebApi.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace E_Commerce.WebApi.Business
{
    public class SellerBO : ISellerBO
    {
        readonly private IAdminReadRepository _adminReadRepository;
        readonly private IAdminWriteRepository _adminWriteRepository;

        readonly private ICustomerReadRepository _customerReadRepository;
        readonly private ICustomerWriteRepository _customerWriteRepository;

        readonly private ISellerReadRepository _sellerReadRepository;
        readonly private ISellerWriteRepository _sellerWriteRepository;

        readonly private IProductReadRepository _productReadRepository;
        readonly private IProductWriteRepository _productWriteRepository;

        readonly private IConfiguration _configuration;

        public SellerBO(IAdminReadRepository adminReadRepository, IAdminWriteRepository adminWriteRepository, ICustomerReadRepository customerReadRepository, ICustomerWriteRepository customerWriteRepository, ISellerReadRepository sellerReadRepository, ISellerWriteRepository sellerWriteRepository, IConfiguration configuration, IProductReadRepository productReadRepository, IProductWriteRepository productWriteRepository)
        {
            _adminReadRepository = adminReadRepository;
            _adminWriteRepository = adminWriteRepository;
            _customerReadRepository = customerReadRepository;
            _customerWriteRepository = customerWriteRepository;
            _sellerReadRepository = sellerReadRepository;
            _sellerWriteRepository = sellerWriteRepository;
            _configuration = configuration;
            _productReadRepository = productReadRepository;
            _productWriteRepository = productWriteRepository;
        }

        public async Task<SellerDto> Create(SellerDto SellerModel)
        {
            var seller = new Seller()
            {

                FirstName = SellerModel.FirstName,
                LastName = SellerModel.LastName,
                Address = SellerModel.Address,
                PhoneNumber = SellerModel.PhoneNumber,
                username = SellerModel.Username,
                Email = SellerModel.Email,
                Password = SellerModel.Password,
                IsApprove = SellerModel.IsApprove,
                CompanyType = SellerModel.CompanyType,
                TaxpayerIDNumber = SellerModel.TaxpayerIDNumber

            };
            await _sellerWriteRepository.AddAsync(seller);
            await _sellerWriteRepository.SaveAsync();
            return SellerModel;
        }
       

        public List<SellerDto> GetAll()
        {
            var sellers = _sellerReadRepository.GetAll().ToList();
            var sellerList = new List<SellerDto>();
            foreach (var seller in sellers)
            {
                var srList = new SellerDto()
                {
                    ID = seller.ID,
                    FirstName = seller.FirstName,
                    LastName = seller.LastName,
                    Address = seller.Address,
                    PhoneNumber = seller.PhoneNumber,
                    Email = seller.Email,
                    Password = seller.Password,
                    Username =seller.username,
                    CompanyType = seller.CompanyType,
                    TaxpayerIDNumber = seller.TaxpayerIDNumber,
                    IsApprove = seller.IsApprove,
                };
                sellerList.Add(srList);
            }
            return sellerList;
        }
     
        public async Task<SellerDto> GetByID(int ID, bool tracking = true)
        {
            var sellers = await _sellerReadRepository.GetByIDAsync(ID);
            var seller = new SellerDto()
            {
                ID = sellers.ID,
                FirstName = sellers.FirstName,
                LastName = sellers.LastName,
                Address = sellers.Address,
                PhoneNumber = sellers.PhoneNumber,
                Email = sellers.Email,
                Password = sellers.Password,
                Username=sellers.username,
                CompanyType = sellers.CompanyType,
                TaxpayerIDNumber = sellers.TaxpayerIDNumber
            };
            return seller;
        }

        public async Task RemoveAsync(int ID)
        {
            var seller = await _sellerReadRepository.GetByIDAsync(ID);
            var sellerRemove = new SellerModel()
            {
                ID = seller.ID,
                FirstName = seller.FirstName,
                LastName = seller.LastName,
                Address = seller.Address,
                PhoneNumber = seller.PhoneNumber,
                Email = seller.Email,
                Password = seller.Password,
                Username = seller.username,
                Role = seller.Role,
                CompanyType = seller.CompanyType,
                TaxpayerIDNumber = seller.TaxpayerIDNumber
            };
            await _sellerWriteRepository.RemoveAsync(ID);
            await _sellerWriteRepository.SaveAsync();
        }

        public async Task<int> SaveAsync()
        {
            var seller = await _sellerWriteRepository.SaveAsync();
            return seller;
        }

        public async Task UpdateAsync(SellerDto seller)
        {
            var sellers = _sellerReadRepository.GetAll().FirstOrDefault(x => x.ID == seller.ID);

            sellers.FirstName = seller.FirstName;
            sellers.LastName = seller.LastName;
            sellers.Address = seller.Address;
            sellers.PhoneNumber = seller.PhoneNumber;
            sellers.username = seller.Username;
            sellers.Email = seller.Email;
            sellers.Password = seller.Password;
           
            seller.CompanyType = seller.CompanyType;
            sellers.TaxpayerIDNumber = seller.TaxpayerIDNumber;

            _sellerWriteRepository.Update(sellers);
            await _sellerWriteRepository.SaveAsync();
        }
       


        #region RegisAndLogin
        public async Task<SellerDto> Registration(SellerDto sellerDto)
        {
            var adminExists = await _adminReadRepository.GetWhere(x => x.Email == sellerDto.Email).FirstOrDefaultAsync();
            var sellerExists = await _sellerReadRepository.GetWhere(x => x.Email == sellerDto.Email).FirstOrDefaultAsync();
            var customerExists = await _customerReadRepository.GetWhere(x=>x.Email == sellerDto.Email).FirstOrDefaultAsync();
            if (sellerExists == null&&customerExists ==null&&adminExists==null)
            {
                var seller = new Seller()
                {
                    FirstName = sellerDto.FirstName,
                    LastName = sellerDto.LastName,
                    Address = sellerDto.Address,
                    Email = sellerDto.Email,
                    Password = sellerDto.Password,
                    username = sellerDto.Username,
                    PhoneNumber = sellerDto.PhoneNumber,
                    CompanyType = sellerDto.CompanyType,
                    IsApprove = sellerDto.IsApprove,
                    TaxpayerIDNumber = sellerDto.TaxpayerIDNumber,
                    Role = RoleType.Seller.ToString()
                };
                await _sellerWriteRepository.AddAsync(seller);
                await _sellerWriteRepository.SaveAsync();

            }
           


            return sellerDto;
        }
        public string Login(LoginModel loginModel)
        {

            var seller = _sellerReadRepository.GetWhere(x => x.Email == loginModel.Email && x.Password == loginModel.Password).FirstOrDefault();
            if (seller == null)
            {
                return "";
            } else if (seller.IsApprove==false) {
                return null;
            }
            var tokenclaims = new List<Claim>
            {
               new Claim(ClaimTypes.NameIdentifier,seller.ID.ToString()),
               new Claim(ClaimTypes.Role,seller.Role,RoleType.Customer.ToString()),
               new Claim(ClaimTypes.Name,seller.Email),
               new Claim(ClaimTypes.Name,seller.Password),
               new Claim(ClaimTypes.Name,seller.ID.ToString())    
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


        public async Task ActiveProduct(int ID) 
        {
            var activeProduct = await _productReadRepository.GetByIDAsync(ID);
            if (activeProduct != null)
            {
                activeProduct.IsProductActive = true;
                _productWriteRepository.Update(activeProduct);
                await _productWriteRepository.SaveAsync();
            }
        }
        public async Task PassiveProduct(int ID) 
        {
            var passiveProduct = await _productReadRepository.GetByIDAsync(ID);
            if (passiveProduct != null)
            {
                passiveProduct.IsProductActive = false;
                _productWriteRepository.Update(passiveProduct);
                await _productWriteRepository.SaveAsync();
            }
        }
    }
}
