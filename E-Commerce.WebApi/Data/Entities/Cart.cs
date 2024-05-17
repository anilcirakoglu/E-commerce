using E_Commerce.WebApi.Data.Entities.Common;
using E_Commerce.WebApi.Data.Entities.Enums;

namespace E_Commerce.WebApi.Data.Entities
{
    public class Cart:BaseEntitiy
    {
        public int ProductID { get; set; }
        public int CustomerID { get; set; }

        public int Quantity { get; set; }//aynı üründen kaç tane eklenmiş

        public CartStatus Status {  get; set; }


        public Product Product { get; set; }
        public Customer Customer { get; set; }
    }
}
