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
    public class ProductBO : IProductBO
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

        public ProductBO(IProductReadRepository productReadRepository, IProductWriteRepository productWriteRepository, ICartReadRepository cartReadRepository, ICartWriteRepository cartWriteRepository, ICustomerReadRepository customerReadRepository, ICustomerWriteRepository customerWriteRepository, ISellerReadRepository sellerReadRepository, ISellerWriteRepository sellerWriteRepository, IStockProductReadRepository stockProductReadRepository, IStockProductWriteRepository stockProductWriteRepository, ICategoryProductReadRepository categoryProductReadRepository, ICategoryProductWriteRepository categoryProductWriteRepository)
        {
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
            _categoryProductReadRepository = categoryProductReadRepository;
            _categoryProductWriteRepository = categoryProductWriteRepository;
        }




        #region add

        public async Task<ProductDto> Create(ProductDto productModel)
        {

            var product = new Product()
            {

                ProductName = productModel.ProductName,
                ProductInformation = productModel.ProductInformation,
                ProductPrice = productModel.ProductPrice,
                Image = productModel.Image,
                discountPercentage = productModel.DiscountPercentage,
                IsProductActive = productModel.IsProductActive,
                CategoryID = productModel.CategoryID,
                IsApprovedProduct = productModel.IsApprovedProduct,
                SellerID = productModel.SellerID,


            }; await _productWriteRepository.AddAsync(product);
            await _productWriteRepository.SaveAsync();

            var stockProduct = new StockProduct()
            {
                ProductID = product.ID,
                ProductQuantity = productModel.ProductQuantity


            };



            await _stockProductWriteRepository.AddAsync(stockProduct);
            await _stockProductWriteRepository.SaveAsync();


            return productModel;
        }
        #endregion



        public List<GetAllProductsForAdmin> GetAllProductsForAdmin() //linq
        {
            var product = _productReadRepository.GetAll();
            var category = _categoryProductReadRepository.GetAll();
            var stock = _stockProductReadRepository.GetAll();
            var seller = _sellerReadRepository.GetAll();
           
            //var allProductlist = new List<GetAllProducts>();
            //foreach (var products in product)
            //{
            //    var productList = new GetAllProducts()
            //    {
            //        ID = products.ID,
            //        ProductName = products.ProductName,
            //        ProductInformation = products.ProductInformation,
            //        ProductPrice = products.ProductPrice,
            //        IsProductActive = products.IsProductActive,
            //        CategoryName = category.FirstOrDefault(x => x.ID == products.CategoryID).CategoryName,
            //        DiscountPercentage = products.discountPercentage,
            //        ProductQuantity = stock.FirstOrDefault(x => x.ProductID == products.ID).ProductQuantity
            //    };
            //    allProductlist.Add(productList);
            //}

            var list = (from products in product
                         join categorys in category on products.CategoryID equals categorys.ID
                         join stocks in stock on products.ID equals stocks.ProductID
                         join sellers in seller on products.SellerID equals sellers.ID
                         select new GetAllProductsForAdmin
                         {
                             ID=products.ID,
                             ProductName = products.ProductName,
                             ProductInformation = products.ProductInformation,
                             ProductPrice = products.ProductPrice,
                             IsProductActive = products.IsProductActive,
                             CategoryName = categorys.CategoryName,
                             DiscountPercentage = products.discountPercentage,
                             ProductQuantity = stocks.ProductQuantity,
                             SellerName = sellers.FirstName,
                             IsApprovedProduct = products.IsApprovedProduct,
                             
                             

                             
                         }).ToList();
            return list;
            //return allProductlist;
        }
        public List<ProductDto> sellerProducts(int ID) //linq
        {
            var product = _productReadRepository.GetAll();
            var category = _categoryProductReadRepository.GetAll();
            var stock = _stockProductReadRepository.GetAll();
            var seller = _sellerReadRepository.GetAll();

         

            var list2 = (from products in product
                         join categorys in category on products.CategoryID equals categorys.ID
                         join stocks in stock on products.ID equals stocks.ProductID
                         join sellers in seller on products.SellerID equals sellers.ID where products.SellerID == ID
                         select new ProductDto
                         {
                             ProductName = products.ProductName,
                             ProductInformation = products.ProductInformation,
                             ProductPrice = products.ProductPrice,
                             IsProductActive = products.IsProductActive,
                             CategoryName = categorys.CategoryName,
                             DiscountPercentage = products.discountPercentage,
                             ProductQuantity = stocks.ProductQuantity,
                             IsApprovedProduct = products.IsApprovedProduct,
                             
                         }).ToList();
            return list2;
            //return allProductlist;
        }


        public List<ProductModel> GetAll()
        {
            var products = _productReadRepository.GetAll().ToList();
            var Quantity = _stockProductReadRepository.GetAll().ToList();
            var category = _categoryProductReadRepository.GetAll().ToList();



            var productList = new List<ProductModel>();
            foreach (var product in products)
            {
                var prlist = new ProductModel()
                {
                    ID = product.ID,
                    ProductName = product.ProductName,
                    ProductInformation = product.ProductInformation,
                    ProductPrice = product.ProductPrice,
                    DiscountPercentage = product.discountPercentage,
                    IsProductActive = product.IsProductActive,
                    CategoryID = product.CategoryID,
                    SellerID = product.SellerID,
                    ProductQuantity = Quantity.FirstOrDefault(x => x.ProductID == product.ID).ProductQuantity// kontrol et

                };
                productList.Add(prlist);
            }
            return productList;
        }

        public async Task<ProductModel> GetByID(int ID, bool tracking = true)
        {
            var products = await _productReadRepository.GetByIDAsync(ID);
            var product = new ProductModel()
            {
                ID = products.ID,
                ProductName = products.ProductName,
                ProductInformation = products.ProductInformation,
                ProductPrice = products.ProductPrice,
                DiscountPercentage = products.discountPercentage,
                PreviousPrice = products.previousPrice,
                IsProductActive = products.IsProductActive,
                CategoryID = products.CategoryID,

            };
            return product;
        }


        public async Task RemoveAsync(int ID)
        {
            var product = await _productReadRepository.GetByIDAsync(ID);
            var productremove = new ProductModel()
            {
                ID = product.ID,
                ProductName = product.ProductName,
                ProductInformation = product.ProductInformation,
                ProductPrice = product.ProductPrice,
                DiscountPercentage = product.discountPercentage,
                PreviousPrice = product.previousPrice,
                IsProductActive = product.IsProductActive,
                CategoryID = product.CategoryID,
                SellerID = product.SellerID,
            };
            await _productWriteRepository.RemoveAsync(ID);
            await _productWriteRepository.SaveAsync();
        }

        public async Task<int> SaveAsync()
        {
            var product = await _productWriteRepository.SaveAsync();
            return product;
        }

        public async Task UpdateAsync(ProductModel product)//cust id ve seller id değişmeyeceği için dto olsuturduğunda modeli değiştir
        {
            var products = _productReadRepository.GetAll().FirstOrDefault(x => x.ID == product.ID);
            if (products != null)
            {
                products.ProductName = product.ProductName;
                products.ProductInformation = product.ProductInformation;
                products.ProductPrice = product.ProductPrice;
                products.previousPrice = product.PreviousPrice;
                products.discountPercentage = product.DiscountPercentage;
                products.IsProductActive = product.IsProductActive;
            }

            _productWriteRepository.Update(products);
            await _productWriteRepository.SaveAsync();
        }
    }
}
