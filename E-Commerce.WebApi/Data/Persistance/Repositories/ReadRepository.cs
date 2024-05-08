using E_Commerce.WebApi.Application;
using E_Commerce.WebApi.Data.Entities.Common;
using E_Commerce.WebApi.Data.Persistance.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace E_Commerce.WebApi.Data.Persistance.Repositories
{
    public class ReadRepository<T>: IReadRepository<T> where T : BaseEntitiy
    {
        private readonly AppDbContext _context;
    
        public ReadRepository(AppDbContext context)
        {
            _context = context;
        }
        public DbSet<T> Table =>_context.Set<T>();
        public IQueryable<T> GetAll(bool tracking = true)
        {
            var query =Table.AsQueryable();
            if(!tracking)
                query=query.AsNoTracking();

            query =query.OrderBy(x=>x.ID);
            return query;
        }
        public static IQueryable<T> ApplyIncludes(IQueryable<T> query, params Expression<Func<T, object>>[] includes) {

            if (includes != null)
            {
                foreach(var includeItem in includes)
                {
                    query=query.Include(includeItem);
                }
            }
            return query;

        }

        public async Task<T>GetByIDAsync(int ID,bool tracking= true)
        {
            var query = Table.AsQueryable();
            if(!tracking) 
                query=query.AsNoTracking();
            return await query.FirstOrDefaultAsync(data=>data.ID==ID);
        }
        public async Task<T>GetSingleAsync(Expression<Func<T, bool>> method,bool tracking = true)
        {
            var query =Table.AsQueryable();
            if(!tracking)
                query=query.AsNoTracking();
            return await query.FirstOrDefaultAsync(method);
        }
        public IQueryable<T> GetWhere(Expression<Func<T,bool>> method,bool tracking = true)
        {
            var query=Table.Where(method);
            if(!tracking)
                query=query.AsNoTracking();
            return query;
        }

       
    }
}
