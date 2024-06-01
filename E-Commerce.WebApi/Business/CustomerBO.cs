using E_Commerce.WebApi.Application.Admins;
using E_Commerce.WebApi.Application.Carts;
using E_Commerce.WebApi.Application.Customers;
using E_Commerce.WebApi.Application.Products;
using E_Commerce.WebApi.Application.Sellers;
using E_Commerce.WebApi.Application.StockProducts;
using E_Commerce.WebApi.Business.Enums;
using E_Commerce.WebApi.Business.Models;
using E_Commerce.WebApi.Data.Entities;
using E_Commerce.WebApi.Data.Entities.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace E_Commerce.WebApi.Business
{
    public class CustomerBO : ICustomerBO
    {
        readonly private IAdminReadRepository _adminReadRepository;
        readonly private IAdminWriteRepository _adminWriteRepository;

        readonly private ICustomerReadRepository _customerReadRepository;
        readonly private ICustomerWriteRepository _customerWriteRepository;

        readonly private ISellerReadRepository _sellerReadRepository;
        readonly private ISellerWriteRepository _sellerWriteRepository;

        readonly private ICartReadRepository _cartReadRepository;
        readonly private ICartWriteRepository _cartWriteRepository;

        readonly private IProductReadRepository _productReadRepository;
        readonly private IProductWriteRepository _productWriteRepository;

        readonly private IStockProductReadRepository _stockProductReadRepository;
        readonly private IStockProductWriteRepository _stockProductWriteRepository;


        readonly private IConfiguration _configuration;

        public CustomerBO(IAdminReadRepository adminReadRepository, IAdminWriteRepository adminWriteRepository, ICustomerReadRepository customerReadRepository, ICustomerWriteRepository customerWriteRepository, ISellerReadRepository sellerReadRepository, ISellerWriteRepository sellerWriteRepository, ICartReadRepository cartReadRepository, ICartWriteRepository cartWriteRepository, IProductReadRepository productReadRepository, IProductWriteRepository productWriteRepository, IConfiguration configuration, IStockProductReadRepository stockProductReadRepository, IStockProductWriteRepository stockProductWriteRepository)
        {
            _adminReadRepository = adminReadRepository;
            _adminWriteRepository = adminWriteRepository;
            _customerReadRepository = customerReadRepository;
            _customerWriteRepository = customerWriteRepository;
            _sellerReadRepository = sellerReadRepository;
            _sellerWriteRepository = sellerWriteRepository;
            _cartReadRepository = cartReadRepository;
            _cartWriteRepository = cartWriteRepository;
            _productReadRepository = productReadRepository;
            _productWriteRepository = productWriteRepository;
            _configuration = configuration;
            _stockProductReadRepository = stockProductReadRepository;
            _stockProductWriteRepository = stockProductWriteRepository;
        }

        public async Task<CustomerModel> Create(CustomerModel customerModel)
        {
            var customer = new Customer()
            {
                ID = customerModel.ID,
                FirstName = customerModel.FirstName,
                LastName = customerModel.LastName,
                Address = customerModel.Address,
                Email = customerModel.Email,
                Password = customerModel.Password,
                PhoneNumber = customerModel.PhoneNumber,
                Role = RoleType.Customer.ToString(),
            };
            await _customerWriteRepository.AddAsync(customer);
            await _customerWriteRepository.SaveAsync();
            return customerModel;
        }

        public List<CustomerModel> GetAll()
        {
            var customers = _customerReadRepository.GetAll().ToList();
            var customerList = new List<CustomerModel>();
            foreach (var customer in customers)
            {
                var cstlist = new CustomerModel()
                {
                    ID = customer.ID,
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    Address = customer.Address,
                    Email = customer.Email,
                    Password = customer.Password,
                    PhoneNumber = customer.PhoneNumber,
                    Role = RoleType.Customer.ToString(),
                };
                customerList.Add(cstlist);
            }
            return customerList;
        }

        public async Task<CustomerModel> GetByID(int ID, bool tracking = true)
        {
            var customers = await _customerReadRepository.GetByIDAsync(ID);
            var customer = new CustomerModel()
            {
                ID = customers.ID,
                FirstName = customers.FirstName,
                LastName = customers.LastName,
                Address = customers.Address,
                Email = customers.Email,
                Password = customers.Password,
                PhoneNumber = customers.PhoneNumber,
            };
            return customer;
        }

        public async Task RemoveAsync(int ID)
        {
            var customer = await _customerReadRepository.GetByIDAsync(ID);
            var customerRemove = new CustomerModel()
            {
                ID = customer.ID,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Address = customer.Address,
                Email = customer.Email,
                Password = customer.Password,
                PhoneNumber = customer.PhoneNumber,
                Role = RoleType.Customer.ToString(),
            };
            await _customerWriteRepository.RemoveAsync(ID);
            await _customerWriteRepository.SaveAsync();
        }

        public async Task<int> SaveAsync()
        {
            var customer = await _customerWriteRepository.SaveAsync();
            return customer;
        }

        public async Task UpdateAsync(CustomerDto customer)
        {

            var customers = _customerReadRepository.GetAll().FirstOrDefault(x => x.ID == customer.ID);

            customers.FirstName = customer.FirstName;
            customers.LastName = customer.LastName;
            customers.Address = customer.Address;
            customers.Email = customer.Email;
            customers.Password = customer.Password;
            customers.PhoneNumber = customer.PhoneNumber;

            _customerWriteRepository.Update(customers);
            await _customerWriteRepository.SaveAsync();



        }
        #region RegisAndLogin
        public async Task<CustomerDto> Registration(CustomerDto customerDto)
        {
            var adminExists = await _adminReadRepository.GetWhere(x => x.Email == customerDto.Email).FirstOrDefaultAsync();
            var customerExists = await _customerReadRepository.GetWhere(x => x.Email == customerDto.Email).FirstOrDefaultAsync();
            var sellerExists = await _sellerReadRepository.GetWhere(x => x.Email == customerDto.Email).FirstOrDefaultAsync();

            if (customerExists == null && sellerExists == null && adminExists == null)
            {
                var customer = new Customer()
                {
                    FirstName = customerDto.FirstName,
                    LastName = customerDto.LastName,
                    Address = customerDto.Address,
                    Email = customerDto.Email,
                    Password = customerDto.Password,
                    PhoneNumber = customerDto.PhoneNumber,
                    Role = RoleType.Customer.ToString()
                };
                await _customerWriteRepository.AddAsync(customer);
                await _customerWriteRepository.SaveAsync();

            }
            return customerDto;
        }

        public string Login(LoginModel loginModel)
        {

            var customer = _customerReadRepository.GetWhere(x => x.Email == loginModel.Email && x.Password == loginModel.Password).FirstOrDefault();

            if (customer == null)
            {
                return "";

            }
            var tokenclaims = new List<Claim>
            {
               new Claim(ClaimTypes.NameIdentifier,customer.ID.ToString()),
               new Claim(ClaimTypes.Role,customer.Role,RoleType.Customer.ToString()),
               new Claim(ClaimTypes.Name,customer.Email),
               new Claim(ClaimTypes.Name,customer.Password),
               new Claim(ClaimTypes.Name,customer.ID.ToString())
           };

            var token = GenerateTokens(tokenclaims);

            return token;


        }
        public string GenerateTokens(IEnumerable<Claim> claims)
        {
            var autSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:KEY"]));
            if (autSigningKey.KeySize < 128)
            {
                using var hmac = new HMACSHA256();
                autSigningKey = new SymmetricSecurityKey(hmac.Key);
            }
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Issuer"],
                Expires = DateTime.UtcNow.AddHours(3),
                SigningCredentials = new SigningCredentials(autSigningKey, SecurityAlgorithms.HmacSha256),
                Subject = new ClaimsIdentity(claims),
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        #endregion

        public async Task AddProductCart(CartDto cartDto)
        {


            var CartProduct = await _cartReadRepository.GetAll().Where(x => x.CustomerID == cartDto.CustomerID && x.ProductID == cartDto.ProductID && x.Status == CartStatus.Active).FirstOrDefaultAsync();
            if (CartProduct == null)
            {
                var cart = new Cart()
                {
                    ProductID = cartDto.ProductID,
                    CustomerID = cartDto.CustomerID,
                    Status = CartStatus.Active,
                    Quantity = 1
                };
                await _cartWriteRepository.AddAsync(cart);
            }
            else
            {
                CartProduct.Quantity += 1;
                _cartWriteRepository.Update(CartProduct);
            }

            await _cartWriteRepository.SaveAsync();
        }


        public async Task DecreaseProductCart(int ID)
        {
            var cartProduct = await _cartReadRepository.GetByIDAsync(ID);
            if (cartProduct == null)
            {
            }
            else if (cartProduct.Quantity == 1)
            {
                await _cartWriteRepository.RemoveAsync(ID);
            }
            else
            {
                cartProduct.Quantity -= 1;
                _cartWriteRepository.Update(cartProduct);
            }
            await _cartWriteRepository.SaveAsync();
        }

        public List<CartListDto> CartList(int ID)
        {
            var customer = _customerReadRepository.GetAll();
            var cart = _cartReadRepository.GetAll();
            var product = _productReadRepository.GetAll();


            var list = (from products in product
                        join carts in cart on products.ID equals carts.ProductID
                        join customers in customer on carts.CustomerID equals customers.ID
                        where customers.ID == ID && carts.Status==CartStatus.Active
                        select new CartListDto
                        {
                            ProductID = products.ID,
                            ProductName = products.ProductName,
                            Quantity = carts.Quantity,
                            Price = carts.Quantity * products.ProductPrice,
                      //      TotalPrice= carts.Quantity*products.ProductPrice,
                        }).Distinct().ToList();


            return list;
        }
        public List<List<PurchaseProductModel>> PurchasedProduct(int ID) {
         List<List<PurchaseProductModel>> purcaseProductListModels = new List< List<PurchaseProductModel>>();

          
            var carts = _cartReadRepository.GetWhere(x=>x.CustomerID== ID && x.Status ==CartStatus.Purchased).GroupBy(x=>x.PurchaseDate).ToList();
            var product = _productReadRepository.GetAll().ToList() ;

            foreach (var cart in carts) {
            var y=  cart.Select(x=> new PurchaseProductModel {
                Price = x.Price,
                ProductID = x.ProductID,
               PurcaseDate =x.PurchaseDate,
               Quantity =x.Quantity,
               ProductName = product.FirstOrDefault(y=>y.ID ==x.ProductID).ProductName,
            }).ToList();
                purcaseProductListModels.Add(y);
            }
            

            //var list = (from products in product
            //            join carts in cart on products.ID equals carts.ProductID
            //            join customers in customer on carts.CustomerID equals customers.ID
            //            where customers.ID == ID && carts.Status == CartStatus.Purchased
            //            select new CartListDto
            //            {
            //                ProductID = products.ID,
            //                ProductName = products.ProductName,
            //                Quantity = carts.Quantity,
            //                Price = carts.Quantity * products.ProductPrice,

            //            }).ToList();

            return purcaseProductListModels;
        }

        public async Task Purchase(int CustomerID)
        {
            DateTime time = DateTime.UtcNow;
            
            var carts = _cartReadRepository.GetWhere(x => x.CustomerID == CustomerID && x.Status == CartStatus.Active).ToList();
            foreach(var cart in carts)
            {
                var stockproduct = await _stockProductReadRepository.GetWhere(x => x.ProductID == cart.ProductID).FirstOrDefaultAsync();
                if (stockproduct.ProductQuantity < cart.Quantity) {

                    throw new Exception("Stock Error");
                }
            }
            foreach (var cart in carts)
            {

                var stockproduct = await _stockProductReadRepository.GetWhere(x => x.ProductID == cart.ProductID).FirstOrDefaultAsync();
                var product = await _productReadRepository.GetByIDAsync(cart.ProductID);
                stockproduct.ProductQuantity -= cart.Quantity;
                _stockProductWriteRepository.Update(stockproduct);
                await _stockProductWriteRepository.SaveAsync();

                cart.Status = CartStatus.Purchased;
                cart.PurchaseDate = time;
                cart.Price = product.ProductPrice;
                _cartWriteRepository.Update(cart);
               await _cartWriteRepository.SaveAsync();
            }

        }


    }






}
