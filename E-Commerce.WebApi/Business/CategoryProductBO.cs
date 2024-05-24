using E_Commerce.WebApi.Application.Carts;
using E_Commerce.WebApi.Application.CategoryProducts;
using E_Commerce.WebApi.Application.Customers;
using E_Commerce.WebApi.Application.Products;
using E_Commerce.WebApi.Application.Sellers;
using E_Commerce.WebApi.Application.StockProducts;
using E_Commerce.WebApi.Business.Models;
using E_Commerce.WebApi.Data.Entities;
using E_Commerce.WebApi.Data.Persistance.Repositories.Customers;

namespace E_Commerce.WebApi.Business
{
    public class CategoryProductBO : ICategoryProductBO
    {
        readonly private IProductReadRepository _productReadRepository;
        readonly private IProductWriteRepository _productWriteRepository;
        readonly private ICartReadRepository _cartReadRepository;
        readonly private ICartWriteRepository _cartWriteRepository;
        readonly private ICustomerReadRepository _customerReadRepository;
        readonly private ICustomerWriteRepository _customerWriteRepository;
        readonly private ISellerReadRepository _sellerReadRepository;
        readonly private ISellerWriteRepository _sellerWriteRepository;
        readonly private IStockProductReadRepository _stockProductReadRepository;
        readonly private IStockProductWriteRepository _stockProductWriteRepository;
        readonly private ICategoryProductReadRepository _categoryProductReadRepository;
        readonly private ICategoryProductWriteRepository _categoryProductWriteRepository;

        public CategoryProductBO(ICategoryProductReadRepository categoryProductReadRepository, ICategoryProductWriteRepository categoryProductWriteRepository, IProductReadRepository productReadRepository, IProductWriteRepository productWriteRepository, ICartReadRepository cartReadRepository, ICartWriteRepository cartWriteRepository, ICustomerReadRepository customerReadRepository, ICustomerWriteRepository customerWriteRepository, ISellerReadRepository sellerReadRepository, ISellerWriteRepository sellerWriteRepository, IStockProductReadRepository stockProductReadRepository, IStockProductWriteRepository stockProductWriteRepository)
        {
            _categoryProductReadRepository = categoryProductReadRepository;
            _categoryProductWriteRepository = categoryProductWriteRepository;
            _productReadRepository = productReadRepository;
            _productWriteRepository = productWriteRepository;
            _cartReadRepository = cartReadRepository;
            _cartWriteRepository = cartWriteRepository;
            _customerReadRepository = customerReadRepository;
            _customerWriteRepository = customerWriteRepository;
            _sellerReadRepository = sellerReadRepository;
            _sellerWriteRepository = sellerWriteRepository;
            _stockProductReadRepository = stockProductReadRepository;
            _stockProductWriteRepository = stockProductWriteRepository;
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
            }
            return cproductList;
        }

        public async Task<CategoryProductModel> GetByID(int ID, bool tracking = true)
        {
            var categorys = await _categoryProductReadRepository.GetByIDAsync(ID);
            var category = new CategoryProductModel()
            {
                ID = categorys.ID,
                CategoryName = categorys.CategoryName,
            };
            return category;
        }

        public List<AllProducts> CategoryList(int categoryID) {
            var product = _productReadRepository.GetAll();
            var category = _categoryProductReadRepository.GetAll();
            var stock = _stockProductReadRepository.GetAll();
            var seller = _sellerReadRepository.GetAll();

            var list = (from products in product
                        join categories in category on products.CategoryID equals categories.ID
                        join stocks in stock on products.ID equals stocks.ProductID
                        join sellers in seller on products.SellerID equals sellers.ID where
                        categories.ID== categoryID  && products.IsApprovedProduct == true && products.IsProductActive == true 
                        select new AllProducts 
                        {
                            ID = products.ID,
                            ProductName = products.ProductName,
                            ProductInformation = products.ProductInformation,
                            ProductPrice = products.ProductPrice,
                            IsProductActive = products.IsProductActive,
                            CategoryName = categories.CategoryName,
                            DiscountPercentage = products.discountPercentage,
                            ProductQuantity = stocks.ProductQuantity,
                            SellerName = sellers.FirstName,
                            IsApprovedProduct = products.IsApprovedProduct,
                            Image = products.Image,
                        }).ToList();
            return list;
        }


        public async Task RemoveAsync(int ID)
        {
            var category = await _categoryProductReadRepository.GetByIDAsync(ID);
            var CategoryRemove = new CategoryProductModel()
            {
                ID = category.ID,
                CategoryName = category.CategoryName,
            };
            await _categoryProductWriteRepository.RemoveAsync(ID);
            await _categoryProductWriteRepository.SaveAsync();
        }

        public async Task<int> SaveAsync()
        {
            var category = await _categoryProductWriteRepository.SaveAsync();
            return category;
        }

        public async Task UpdateAsync(CategoryProductModel category)
        {
            var categories = _categoryProductReadRepository.GetAll().FirstOrDefault(x => x.ID == category.ID);
            if (categories != null)
            {
                categories.CategoryName = category.CategoryName;
                _categoryProductWriteRepository.Update(categories);
                await _categoryProductWriteRepository.SaveAsync();
            }



        }
    }
}
