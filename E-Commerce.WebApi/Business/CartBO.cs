﻿using E_Commerce.WebApi.Application.Carts;
using E_Commerce.WebApi.Business.Models;
using E_Commerce.WebApi.Data.Entities;

namespace E_Commerce.WebApi.Business
{
    public class CartBO : ICartBO
    {
        readonly private ICartReadRepository _cartReadRepository;
        readonly private ICartWriteRepository _cartWriteRepository;
        public CartBO(ICartReadRepository cartReadRepository,ICartWriteRepository cartWriteRepository)
        {
            _cartReadRepository = cartReadRepository;
            _cartWriteRepository = cartWriteRepository;
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
    }
}