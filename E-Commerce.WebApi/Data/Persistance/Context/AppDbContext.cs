using E_Commerce.WebApi.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.WebApi.Data.Persistance.Context
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        {

        }
        public DbSet<Product> Product {  get; set; }
        public DbSet<Seller>Seller { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Cart> Cart { get; set; }
        public DbSet<StockProduct> StockProduct { get; set; }
        public DbSet<CategoryProduct> CategoryProduct { get; set; }
        public DbSet<Admin>Admins { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
             base.OnModelCreating(modelBuilder);  
        }
    }
}
