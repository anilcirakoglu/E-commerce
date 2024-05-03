using E_Commerce.WebApi.Application.StockProducts;
using E_Commerce.WebApi.Business.Models;
using E_Commerce.WebApi.Data.Entities;

namespace E_Commerce.WebApi.Business
{
    public class StockProductBO : IStockProductBO
    {
        readonly private IStockProductReadRepository _stockProductReadRepository;
        readonly private IStockProductWriteRepository _stockProductWriteRepository;

        public StockProductBO(IStockProductReadRepository stockProductReadRepository, IStockProductWriteRepository stockProductWriteRepository)
        {
            _stockProductReadRepository = stockProductReadRepository;
            _stockProductWriteRepository = stockProductWriteRepository;
        }

        public async Task<StockProductModel> Create(StockProductModel product)
        {
            var stckProduct = new StockProduct()
            {
                ID = product.ID,
                ProductID = product.ID,
                ProductQuantity = product.ProductQuantity,
            };
            await _stockProductWriteRepository.AddAsync(stckProduct);
            await _stockProductWriteRepository.SaveAsync();
            return product;
        }

        public List<StockProductModel> GetAll()
        {
            var stckproducts = _stockProductReadRepository.GetAll().ToList();
            var stockProductList = new List<StockProductModel>();
            foreach (var stckProduct in stckproducts)
            {
                var stlist = new StockProductModel()
                 {
                     ID=stckProduct.ID,
                     ProductID = stckProduct.ProductID,
                     ProductQuantity = stckProduct.ProductQuantity
                 };
                stockProductList.Add(stlist);
            }
            return stockProductList;
        }

        public async Task<StockProductModel> GetByID(int ID, bool tracking = true)
        {
            var stockproducts = await _stockProductReadRepository.GetByIDAsync(ID);
            var stproduct = new StockProductModel()
            {
                ID = stockproducts.ID,
                ProductID = stockproducts.ProductID,
                ProductQuantity = stockproducts.ProductQuantity
            };
            return stproduct;
        }

        public async Task RemoveAsync(int ID)
        {
            var stproduct = await _stockProductReadRepository.GetByIDAsync(ID);
            var stockRemove = new StockProductModel()
            {
                ID = stproduct.ID,
                ProductID = stproduct.ProductID,
                ProductQuantity = stproduct.ProductQuantity
            };
            await _stockProductWriteRepository.RemoveAsync(ID);
            await _stockProductWriteRepository.SaveAsync();
        }

        public async Task<int> SaveAsync()
        {
            var stproduct = await _stockProductWriteRepository.SaveAsync();
            return stproduct;
        }

        public async Task UpdateAsync(StockProductModel stockProduct)
        {
            var products = _stockProductReadRepository.GetAll().FirstOrDefault(x=>x.ID == stockProduct.ID);

            products.ProductQuantity = stockProduct.ProductQuantity;

            _stockProductWriteRepository.Update(products);
            await _stockProductWriteRepository.SaveAsync();
        }
    }
}
