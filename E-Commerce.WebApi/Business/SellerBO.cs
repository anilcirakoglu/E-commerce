using E_Commerce.WebApi.Application.Sellers;
using E_Commerce.WebApi.Business.Models;
using E_Commerce.WebApi.Data.Entities;

namespace E_Commerce.WebApi.Business
{
    public class SellerBO : ISellerBO
    {
        readonly private ISellerReadRepository _sellerReadRepository;
        readonly private ISellerWriteRepository _sellerWriteRepository;
        public SellerBO(ISellerReadRepository sellerReadRepository, ISellerWriteRepository sellerWriteRepository)
        {
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
    }
}
