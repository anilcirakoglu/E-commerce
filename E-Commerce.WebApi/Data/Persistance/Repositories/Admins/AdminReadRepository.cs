using E_Commerce.WebApi.Application.Admins;
using E_Commerce.WebApi.Data.Entities;
using E_Commerce.WebApi.Data.Persistance.Context;

namespace E_Commerce.WebApi.Data.Persistance.Repositories.Admins
{
    public class AdminReadRepository : ReadRepository<Admin>, IAdminReadRepository
    {
        public AdminReadRepository(AppDbContext context) : base(context)
        {
        }
    }
}
