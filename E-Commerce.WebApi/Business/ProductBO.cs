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
        public List<AllProducts> GetAllProductsForAdmin()
        {
            var product = _productReadRepository.GetAll();
            var category = _categoryProductReadRepository.GetAll();
            var stock = _stockProductReadRepository.GetAll();
            var seller = _sellerReadRepository.GetAll();


            var list = (from products in product
                        join categories in category on products.CategoryID equals categories.ID
                        join stocks in stock on products.ID equals stocks.ProductID
                        join sellers in seller on products.SellerID equals sellers.ID
            
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




                        }).ToList();
            return list;
            
        }
        public List<AllProducts> Search(string name) 
        {
            var product = _productReadRepository.GetAll();
            var category = _categoryProductReadRepository.GetAll();
            var stock = _stockProductReadRepository.GetAll();
            var seller = _sellerReadRepository.GetAll();
            
            var list = (from products in product
                        join categories in category on products.CategoryID equals categories.ID
                        join stocks in stock on products.ID equals stocks.ProductID
                        join sellers in seller on products.SellerID equals sellers.ID
                        where products.ProductName.ToLower() == name.ToLower() || categories.CategoryName.ToLower() == name.ToLower() && products.IsApprovedProduct== true && products.IsProductActive==true &&stocks.ProductQuantity >= 1
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
                            Username =sellers.username,
                            IsApprovedProduct = products.IsApprovedProduct,
                            Image=products.Image,

                        }
                        ).ToList();
            return list;
        }


        public List<AllProducts> GetAllProducts() 
        {
            var product = _productReadRepository.GetAll();
            var category = _categoryProductReadRepository.GetAll();
            var stock = _stockProductReadRepository.GetAll();
            var seller = _sellerReadRepository.GetAll();
           

            var list = (from products in product
                         join categories in category on products.CategoryID equals categories.ID
                         join stocks in stock on products.ID equals stocks.ProductID
                         join sellers in seller on products.SellerID equals sellers.ID where products.IsApprovedProduct== true && products.IsProductActive == true && stocks.ProductQuantity >= 1
                        select new AllProducts
                         {
                             ID=products.ID,
                             ProductName = products.ProductName,
                             ProductInformation = products.ProductInformation,
                             ProductPrice = products.ProductPrice,
                             IsProductActive = products.IsProductActive,
                             CategoryName = categories.CategoryName,
                             DiscountPercentage = products.discountPercentage,
                             ProductQuantity = stocks.ProductQuantity,
                             SellerName = sellers.FirstName,
                             Username =sellers.username,
                             IsApprovedProduct = products.IsApprovedProduct,
                             Image =products.Image
                             

                             
                         }).ToList();
            return list;
            
        }
        public List<ProductDto> sellerProducts(int ID) 
        {
            var product = _productReadRepository.GetAll();
            var category = _categoryProductReadRepository.GetAll();
            var stock = _stockProductReadRepository.GetAll();
            var seller = _sellerReadRepository.GetAll();

         

            var list2 = (from products in product
                         join categories in category on products.CategoryID equals categories.ID
                         join stocks in stock on products.ID equals stocks.ProductID
                         join sellers in seller on products.SellerID equals sellers.ID where products.SellerID == ID
                         select new ProductDto
                         {
                             ID = products.ID,
                             ProductName = products.ProductName,
                             ProductInformation = products.ProductInformation,
                             ProductPrice = products.ProductPrice,
                             IsProductActive = products.IsProductActive,
                             CategoryName = categories.CategoryName,
                             DiscountPercentage = products.discountPercentage,
                             ProductQuantity = stocks.ProductQuantity,
                             IsApprovedProduct = products.IsApprovedProduct,
                             
                         }).ToList();
            return list2;
          
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
        public async Task<ProductDetailForCustomer> DetailForCustomer(int ID) {
            var products = await _productReadRepository.GetByIDAsync(ID);
            var stocks = _stockProductReadRepository.GetAll().Where(x => x.ProductID == products.ID);
            var seller = _sellerReadRepository.GetAll().Where(x=>x.ID==products.SellerID);
            var product = new ProductDetailForCustomer() {

               ID = products.ID,
                ProductName = products.ProductName,
                ProductInformation = products.ProductInformation,
                ProductPrice = products.ProductPrice,
                ProductQuantity = stocks.FirstOrDefault(x => x.ProductID == products.ID).ProductQuantity,
                Username = seller.FirstOrDefault(x => x.ID == products.SellerID).username,
                Img =products.Image
            };
            return product;
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

        public async Task UpdateAsync(ProductDto product)
        {
            var products = _productReadRepository.GetAll().FirstOrDefault(x => x.ID == product.ID);
            var stocks = _stockProductReadRepository.GetAll().FirstOrDefault(x=>x.ID == product.ID);
            if (products != null&&stocks!=null)
            {

                products.ProductName = product.ProductName;
                products.ProductInformation = product.ProductInformation;
                products.ProductPrice = product.ProductPrice;
                products.CategoryID = product.CategoryID;
                products.Image= product.Image;
                products.discountPercentage = product.DiscountPercentage;
                stocks.ProductQuantity = product.ProductQuantity;
            }

            _productWriteRepository.Update(products);
            await _productWriteRepository.SaveAsync();

            _stockProductWriteRepository.Update(stocks);
            await _stockProductWriteRepository.SaveAsync();
        }
    }
}
