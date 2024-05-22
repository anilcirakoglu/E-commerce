using E_Commerce.WebApi.Application.Carts;
using E_Commerce.WebApi.Application.Products;
using E_Commerce.WebApi.Business.Models;
using E_Commerce.WebApi.Data.Entities;

namespace E_Commerce.WebApi.Business
{
    public class CartBO : ICartBO
    {
        readonly private ICartReadRepository _cartReadRepository;
        readonly private ICartWriteRepository _cartWriteRepository;

        readonly private IProductReadRepository _productReadRepository;
        readonly private IProductWriteRepository _productWriteRepository;

        public CartBO(ICartReadRepository cartReadRepository, ICartWriteRepository cartWriteRepository, IProductReadRepository productReadRepository, IProductWriteRepository productWriteRepository)
        {
            _cartReadRepository = cartReadRepository;
            _cartWriteRepository = cartWriteRepository;
            _productReadRepository = productReadRepository;
            _productWriteRepository = productWriteRepository;
        }

        public async Task<CartModel> Create(CartModel cartModel)
        {
            var cart = new Cart()
            {
               ID=cartModel.ID,
               CustomerID=cartModel.CustomerID,
               ProductID=cartModel.ProductID,
            };
            await _cartWriteRepository.AddAsync(cart);
            await _cartWriteRepository.SaveAsync();
            return cartModel;
        }

        public List<CartModel> GetAll()
        {
            var carts = _cartReadRepository.GetAll().ToList();
            var cartList = new List<CartModel>();
            foreach (var cart in carts)
            {
                var crtlist = new CartModel()
                {
                    ID = cart.ID,
                    CustomerID = cart.CustomerID,
                    ProductID = cart.ProductID,
                };
                cartList.Add(crtlist);
            }
            return cartList;
        }
      

        public async Task<CartModel> GetByID(int ID, bool tracking = true)
        {
            var carts = await _cartReadRepository.GetByIDAsync(ID);
            var cart = new CartModel()
            {
                ID = carts.ID,
                CustomerID = carts.CustomerID,
                ProductID = carts.ProductID,
            };
            return cart;
        }

        public async Task RemoveAsync(int ID)
        {
            var cart = await _cartReadRepository.GetByIDAsync(ID);
            var cartRemove = new CartModel()
            {
                ID = cart.ID,
                CustomerID = cart.CustomerID,
                ProductID = cart.ProductID,

            };
            await _cartWriteRepository.RemoveAsync(ID);
            await _cartWriteRepository.SaveAsync();
        }

        public async Task<int> SaveAsync()
        {
            var cart = await _cartWriteRepository.SaveAsync();
            return cart;
        }

        public Task UpdateAsync(CartModel cart)//cart update olmaz bunu kontrol et 
        {
            throw new NotImplementedException();
        }

        public  Cart DecreaseCartProduct(int ProductID) 
        {
            var cart =  _cartReadRepository.GetWhere(x=>x.ProductID==ProductID).FirstOrDefault();
            if(cart != null) 
            {
                if (cart.Quantity > 1)
                {
                    cart.Quantity -= 1;
                    _cartWriteRepository.Update(cart);
                }
                else {
                    _cartWriteRepository.Remove(cart);
                }
               
                _cartWriteRepository.SaveAsync();
            }
            return cart;
        }
        public Cart IncreaseCartProduct(int ProductID) {
        var cart =_cartReadRepository.GetWhere(x=>x.ProductID== ProductID).FirstOrDefault();
            if(cart != null)
            {
                cart.Quantity += 1;
                _cartWriteRepository.Update(cart);
            }

            _cartWriteRepository.SaveAsync();
            return cart;
        
        }



        



    }
}
