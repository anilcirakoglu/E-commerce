using E_Commerce.WebApi.Data.Entities.Common;
using System.Linq.Expressions;
namespace E_Commerce.WebApi.Application
{
    public interface IReadRepository<T> : IRepository<T> where T : BaseEntitiy
    {
        IQueryable<T> GetAll(bool tracking = true);
        IQueryable<T> GetWhere(Expression<Func<T, bool>> method, bool tracking = true);
        Task<T> GetSingleAsync(Expression<Func<T, bool>> method, bool tracking = true);
        Task<T> GetByIDAsync(int id, bool tracking = true);
    }
}
