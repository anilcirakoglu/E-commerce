using E_Commerce.WebApi.Application.Admins;
using E_Commerce.WebApi.Application.Customers;
using E_Commerce.WebApi.Application.Products;
using E_Commerce.WebApi.Application.Sellers;
using E_Commerce.WebApi.Business.Enums;
using E_Commerce.WebApi.Business.Models;
using E_Commerce.WebApi.Data.Entities;
using Microsoft.EntityFrameworkCore;

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

        public SellerBO(IAdminReadRepository adminReadRepository, IAdminWriteRepository adminWriteRepository, ICustomerReadRepository customerReadRepository, ICustomerWriteRepository customerWriteRepository, ISellerReadRepository sellerReadRepository, ISellerWriteRepository sellerWriteRepository)
        {
            _adminReadRepository = adminReadRepository;
            _adminWriteRepository = adminWriteRepository;
            _customerReadRepository = customerReadRepository;
            _customerWriteRepository = customerWriteRepository;
            _sellerReadRepository = sellerReadRepository;
            _sellerWriteRepository = sellerWriteRepository;
        }

        public async Task<SellerModel> Create(SellerModel SellerModel)
        {
            var seller = new Seller()
            {
                ID = SellerModel.ID,
                FirstName = SellerModel.FirstName,
                LastName = SellerModel.LastName,
                Address = SellerModel.Address,
                PhoneNumber = SellerModel.PhoneNumber,
                Email = SellerModel.Email,
                Password = SellerModel.Password,
                Role = SellerModel.Role,
                CompanyType = SellerModel.CompanyType,
                TaxpayerIDNumber = SellerModel.TaxpayerIDNumber

            };
            await _sellerWriteRepository.AddAsync(seller);
            await _sellerWriteRepository.SaveAsync();
            return SellerModel;
        }
       

        public List<SellerModel> GetAll()
        {
            var sellers = _sellerReadRepository.GetAll().ToList();
            var sellerList = new List<SellerModel>();
            foreach (var seller in sellers)
            {
                var srList = new SellerModel()
                {
                    ID = seller.ID,
                    FirstName = seller.FirstName,
                    LastName = seller.LastName,
                    Address = seller.Address,
                    PhoneNumber = seller.PhoneNumber,
                    Email = seller.Email,
                    Password = seller.Password,
                    Role = seller.Role,
                    CompanyType = seller.CompanyType,
                    TaxpayerIDNumber = seller.TaxpayerIDNumber
                };
                sellerList.Add(srList);
            }
            return sellerList;
        }

        public async Task<SellerModel> GetByID(int ID, bool tracking = true)
        {
            var sellers = await _sellerReadRepository.GetByIDAsync(ID);
            var seller = new SellerModel()
            {
                ID = sellers.ID,
                FirstName = sellers.FirstName,
                LastName = sellers.LastName,
                Address = sellers.Address,
                PhoneNumber = sellers.PhoneNumber,
                Email = sellers.Email,
                Password = sellers.Password,
                Role = sellers.Role,
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

        public async Task UpdateAsync(SellerModel seller)
        {
            var sellers = _sellerReadRepository.GetAll().FirstOrDefault(x => x.ID == seller.ID);

            sellers.FirstName = seller.FirstName;
            sellers.LastName = seller.LastName;
            sellers.Address = seller.Address;
            sellers.PhoneNumber = seller.PhoneNumber;
            sellers.Email = seller.Email;
            sellers.Password = seller.Password;
            sellers.Role = seller.Role;
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
                    PhoneNumber = sellerDto.PhoneNumber,
                    CompanyType = sellerDto.CompanyType,
                    TaxpayerIDNumber = sellerDto.TaxpayerIDNumber,
                    Role = RoleType.Seller.ToString()
                };
                await _sellerWriteRepository.AddAsync(seller);
                await _sellerWriteRepository.SaveAsync();

            }
            //Admin control 


            return sellerDto;
        }
        public string Login(LoginModel loginModel)
        {
            var customer = _sellerReadRepository.GetWhere(x => x.Email == loginModel.Email).FirstOrDefault();
            var customerPassword = _sellerReadRepository.GetWhere(x => x.Password == loginModel.Password).FirstOrDefault();
            if (customer == null || customerPassword == null)
            {
                return "Invalid Email or Password";
            }

            else { return "okey"; }


        }


        #endregion
    }
}
