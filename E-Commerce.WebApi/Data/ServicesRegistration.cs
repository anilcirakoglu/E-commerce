using E_Commerce.WebApi.Application.Admins;
using E_Commerce.WebApi.Application.Carts;
using E_Commerce.WebApi.Application.CategoryProducts;
using E_Commerce.WebApi.Application.Customers;
using E_Commerce.WebApi.Application.Products;
using E_Commerce.WebApi.Application.Sellers;
using E_Commerce.WebApi.Application.StockProducts;
using E_Commerce.WebApi.Business;
using E_Commerce.WebApi.Data.Persistance.Repositories.Admins;
using E_Commerce.WebApi.Data.Persistance.Repositories.Carts;
using E_Commerce.WebApi.Data.Persistance.Repositories.CategoryProducts;
using E_Commerce.WebApi.Data.Persistance.Repositories.Customers;
using E_Commerce.WebApi.Data.Persistance.Repositories.Products;
using E_Commerce.WebApi.Data.Persistance.Repositories.Sellers;
using E_Commerce.WebApi.Data.Persistance.Repositories.StockProducts;
using E_Commerce.WebApi.Data.Persistance.Repositories;

namespace E_Commerce.WebApi.Data
{
    public static class ServicesRegistration
    {
        public static void AddPersistanceServices(this IServiceCollection services)
        {
            services.AddScoped<IProductBO, ProductBO>();
            services.AddScoped<IProductReadRepository, ProductReadRepository>();
            services.AddScoped<IProductWriteRepository, ProductWriteRepository>();

            services.AddScoped<ICartReadRepository, CartReadRepository>();
            services.AddScoped<ICartWriteRepository, CartWriteRepository>();

            services.AddScoped<IAdminReadRepository, AdminReadRepository>();
            services.AddScoped<IAdminWriteRepository, AdminWriteRepository>();

            services.AddScoped<ICategoryProductReadRepository, CategoryProductReadRepository>();
            services.AddScoped<ICategoryProductWriteRepository, CategoryProductWriteRepository>();

            services.AddScoped<ICustomerReadRepository, CustomerReadRepository>();
            services.AddScoped<ICustomerWriteRepository, CustomerWriteRepository>();

            services.AddScoped<ISellerReadRepository, SellerReadRepository>();
            services.AddScoped<ISellerWriteRepository, SellerWriteRepository>();

            services.AddScoped<IStockProductReadRepository, StockProductReadRepository>();
            services.AddScoped<IStockProductWriteRepository, StockProductWriteRepository>();



        }
    }
}
