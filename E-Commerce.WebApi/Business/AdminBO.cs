using E_Commerce.WebApi.Application.Admins;
using E_Commerce.WebApi.Application.Customers;
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
    public class AdminBO : IAdminBO
    {
        readonly private IAdminReadRepository _adminReadRepository;
        readonly private IAdminWriteRepository _adminWriteRepository;

        readonly private ICustomerReadRepository _customerReadRepository;
        readonly private ICustomerWriteRepository _customerWriteRepository;

        readonly private ISellerReadRepository _sellerReadRepository;
        readonly private ISellerWriteRepository _sellerWriteRepository;

        readonly private IConfiguration _configuration;

        public AdminBO(IAdminReadRepository adminReadRepository, IAdminWriteRepository adminWriteRepository, ICustomerReadRepository customerReadRepository, ICustomerWriteRepository customerWriteRepository, ISellerReadRepository sellerReadRepository, ISellerWriteRepository sellerWriteRepository, IConfiguration configuration)
        {
            _adminReadRepository = adminReadRepository;
            _adminWriteRepository = adminWriteRepository;
            _customerReadRepository = customerReadRepository;
            _customerWriteRepository = customerWriteRepository;
            _sellerReadRepository = sellerReadRepository;
            _sellerWriteRepository = sellerWriteRepository;
            _configuration = configuration;
        }

        
       

        public List<AdminModel> GetAll()
        {
            var admins = _adminReadRepository.GetAll().ToList();
            var adminList = new List<AdminModel>();
            foreach (var admin in admins)
            {
                var adlist = new AdminModel()
                {
                    ID = admin.ID,
                    FirstName = admin.FirstName,
                    LastName = admin.LastName,
                    Address = admin.Address,
                    Email = admin.Email,
                    Password = admin.Password,
                    PhoneNumber = admin.PhoneNumber,
                    Role = admin.Role,

                };
                adminList.Add(adlist);
            }
            return adminList;
        }

        public async Task<AdminModel> GetByID(int ID, bool tracking = true)
        {
            var admins = await _adminReadRepository.GetByIDAsync(ID);
            var admin = new AdminModel()
            {
                ID = admins.ID,
                FirstName = admins.FirstName,
                LastName = admins.LastName,
                Address = admins.Address,
                Email = admins.Email,
                Password = admins.Password,
                PhoneNumber = admins.PhoneNumber,
                Role = admins.Role,

            };
            return admin;
        }

        public async Task RemoveAsync(int ID)
        {
            var admin = await _adminReadRepository.GetByIDAsync(ID);
            var adminremove = new AdminModel()
            {
                ID = admin.ID,
                FirstName = admin.FirstName,
                LastName = admin.LastName,
                Address = admin.Address,
                Email = admin.Email,
                Password = admin.Password,
                PhoneNumber = admin.PhoneNumber,
                Role = admin.Role,

            };
            await _adminWriteRepository.RemoveAsync(ID);
            await _adminWriteRepository.SaveAsync();
        }

        public async Task<int> SaveAsync()
        {
            var admin = await _adminWriteRepository.SaveAsync();
            return admin;
        }

        public async Task UpdateAsync(AdminDto adminModel)
        {
            var admins = _adminReadRepository.GetAll().FirstOrDefault(x => x.ID == adminModel.ID);
            admins.FirstName = adminModel.FirstName;
            admins.LastName = adminModel.LastName;
            admins.Address = adminModel.Address;
            admins.Email = adminModel.Email;
            admins.Password = adminModel.Password;
            admins.PhoneNumber = adminModel.PhoneNumber;
           

            _adminWriteRepository.Update(admins);
            await _adminWriteRepository.SaveAsync();
        }

        #region AddSeller
        
        public async Task ApprovedSeller(int ID) 
        {

            var sellerApproved = await _sellerReadRepository.GetByIDAsync(ID);
       
            if (sellerApproved != null)
            {
                sellerApproved.IsApprove = true;
                _sellerWriteRepository.Update(sellerApproved);
                await _sellerWriteRepository.SaveAsync();
            }
                
        }

        #endregion
        






        #region Login
        public string Login(LoginModel loginModel)
        {

            var admin = _adminReadRepository.GetWhere(x => x.Email == loginModel.Email && x.Password == loginModel.Password).FirstOrDefault();

            if (admin == null)
            {
                return "";

            }
            var tokenclaims = new List<Claim>
            {
               new Claim(ClaimTypes.NameIdentifier,admin.ID.ToString()),
               new Claim(ClaimTypes.Role,admin.Role,RoleType.Admin.ToString()),
               new Claim(ClaimTypes.Name,admin.Email),
               new Claim(ClaimTypes.Name,admin.Password),
               new Claim(ClaimTypes.Name,admin.ID.ToString()
               )
              
           };

            var token = GenerateTokens(tokenclaims);

            return token;


        }
        public async Task<AdminDto> Registration(AdminDto adminDto)
        {
            var adminExists = await _adminReadRepository.GetWhere(x => x.Email == adminDto.Email).FirstOrDefaultAsync();
            var sellerExists = await _sellerReadRepository.GetWhere(x => x.Email == adminDto.Email).FirstOrDefaultAsync();
            var customerExists = await _customerReadRepository.GetWhere(x => x.Email == adminDto.Email).FirstOrDefaultAsync();
            if (sellerExists == null && customerExists == null && adminExists == null)
            {
                var admin = new Admin()
                {

                    FirstName = adminDto.FirstName,
                    LastName = adminDto.LastName,
                    Address = adminDto.Address,
                    Email = adminDto.Email,
                    Password = adminDto.Password,
                    PhoneNumber = adminDto.PhoneNumber,
                    Role = RoleType.Admin.ToString()

                };
                await _adminWriteRepository.AddAsync(admin);
                await _adminWriteRepository.SaveAsync();
            }

            return adminDto;
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
    }

}
