using E_Commerce.WebApi.Data.Entities.Common;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.WebApi.Application
{
    public interface IRepository<T> where T : BaseEntitiy
    {
        DbSet<T> Table { get; }
    }
}
