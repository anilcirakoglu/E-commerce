using E_Commerce.WebApi.Application;
using E_Commerce.WebApi.Data.Entities.Common;
using E_Commerce.WebApi.Data.Persistance.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace E_Commerce.WebApi.Data.Persistance.Repositories
{
    public class WriteRepository<T>:IWriteRepository<T> where T : BaseEntitiy
    {
        readonly private AppDbContext _context;
        public WriteRepository(AppDbContext context)
        {
            _context = context;
        }
        public DbSet<T> Table =>_context.Set<T>();

        public async Task<bool>AddAsync(T Model)
        {
            EntityEntry<T> entityEntry=await Table.AddAsync(Model);
            return entityEntry.State == EntityState.Added;
        }
        public async Task<bool>AddRangeAsync(List<T> model)
        {
            await Table.AddRangeAsync(model);
            return true;
        }
        public bool Remove(T Model)
        {
            EntityEntry<T> entityEntry = Table.Remove(Model);
            return entityEntry.State==EntityState.Deleted;
        }
        public async Task<bool>RemoveAsync(int id)
        {
            T Model =await Table.FirstOrDefaultAsync(data=>data.ID==id);
            return Remove(Model);
        }
        public bool RemoveRange(List<T> model)
        {
            Table.RemoveRange(model);
            return true;
        }
        public bool Update(T model)
        {
            EntityEntry entityEntry = Table.Update(model);
            return entityEntry.State == EntityState.Modified;
        }
        public async Task<int>SaveAsync()=>await _context.SaveChangesAsync();

       
    }
}
