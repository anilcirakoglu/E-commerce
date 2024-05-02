using E_Commerce.WebApi.Application.Admins;
using E_Commerce.WebApi.Business.Models;
using E_Commerce.WebApi.Data.Entities;

namespace E_Commerce.WebApi.Business
{
    public class AdminBO : IAdminBO
    {
        readonly private IAdminReadRepository _adminReadRepository;
        readonly private IAdminWriteRepository _adminWriteRepository;
        public AdminBO(IAdminReadRepository adminReadRepository, IAdminWriteRepository adminWriteRepository)
        {
            _adminReadRepository = adminReadRepository;
            _adminWriteRepository = adminWriteRepository;
        }
        public async Task<AdminModel> Create(AdminModel adminModel)
        {
            var admin = new Admin()
            {
                ID = adminModel.ID,
                FirstName = adminModel.FirstName,
                LastName = adminModel.LastName,
                Address = adminModel.Address,
                Email = adminModel.Email,
                Password = adminModel.Password,
                PhoneNumber = adminModel.PhoneNumber,
                Role = adminModel.Role,
            };
            await _adminWriteRepository.AddAsync(admin);
            await _adminWriteRepository.SaveAsync();
            return adminModel;
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

        public async Task UpdateAsync(AdminModel adminModel)
        {
           var admins =_adminReadRepository.GetAll().FirstOrDefault(x=>x.ID == adminModel.ID);
            admins.FirstName = adminModel.FirstName;
            admins.LastName = adminModel.LastName;
            admins.Address = adminModel.Address;
            admins.Email = adminModel.Email;
            admins.Password = adminModel.Password;
            admins.PhoneNumber = adminModel.PhoneNumber;
            admins.Role = adminModel.Role;//role degiştemeez

            _adminWriteRepository.Update(admins);
            await _adminWriteRepository.SaveAsync();
        }
    }
}
