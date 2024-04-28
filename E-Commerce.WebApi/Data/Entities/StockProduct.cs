using E_Commerce.WebApi.Data.Entities.Common;

namespace E_Commerce.WebApi.Data.Entities
{
    public class StockProduct:BaseEntitiy
    {
        public int ProductQuantity { get; set; }
        public int ProductID {  get; set; }

        public Product Product { get; set; }
    }
}
