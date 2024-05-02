using E_Commerce.WebApi.Application.Carts;
using E_Commerce.WebApi.Application.CategoryProducts;
using E_Commerce.WebApi.Application.Customers;
using E_Commerce.WebApi.Application.Products;
using E_Commerce.WebApi.Application.Sellers;
using E_Commerce.WebApi.Application.StockProducts;
using E_Commerce.WebApi.Business.Models;
using E_Commerce.WebApi.Data.Entities;

namespace E_Commerce.WebApi.Business
{
    public class CategoryProductBO : ICategoryProductBO
    {
        //readonly private IProductReadRepository _productReadRepository;
        //readonly private IProductWriteRepository _productWriteRepository;
        //readonly private ICartReadRepository _cartReadRepository;
        //readonly private ICartWriteRepository _cartWriteRepository;
        //readonly private ICustomerReadRepository _customerReadRepository;
        //readonly private ICustomerWriteRepository _customerWriteRepository;
        //readonly private ISellerReadRepository _sellerReadRepository;
        //readonly private ISellerWriteRepository _sellerWriteRepository;
        //readonly private IStockProductReadRepository _stockProductReadRepository;
        //readonly private IStockProductWriteRepository _stockProductWriteRepository;
        readonly private ICategoryProductReadRepository _categoryProductReadRepository;
        readonly private ICategoryProductWriteRepository _categoryProductWriteRepository;

        public CategoryProductBO(ICategoryProductReadRepository categoryProductReadRepository,ICategoryProductWriteRepository categoryProductWriteRepository)
        {
            _categoryProductReadRepository = categoryProductReadRepository;
            _categoryProductWriteRepository= categoryProductWriteRepository;
        }

      
        public async Task<CategoryProductModel> Create(CategoryProductModel product)
        {
            var categoryProduct = new CategoryProduct()
            {
                ID = product.ID,
                CategoryName = product.CategoryName,
                
            };
            await _categoryProductWriteRepository.AddAsync(categoryProduct);
            await _categoryProductWriteRepository.SaveAsync();
            return product;
        }

        public List<CategoryProductModel> GetAll()
        {
           var cproduct = _categoryProductReadRepository.GetAll().ToList();
           var cproductList = new List<CategoryProductModel>();
            foreach (var categoryProduct in cproduct)
            {
                var prlist = new CategoryProductModel()
                {
                    ID = categoryProduct.ID,
                    CategoryName = categoryProduct.CategoryName,
                };
                cproductList.Add(prlist);
            }return cproductList;
        }

        public Task<CategoryProductModel> GetByID(int ID, bool tracking = true)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(int ID)
        {
            throw new NotImplementedException();
        }

        public Task<int> SaveAsync()
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(CategoryProductModel cart)
        {
            throw new NotImplementedException();
        }
    }
}
