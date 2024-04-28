using E_Commerce.WebApi.Data.Entities.Common;

namespace E_Commerce.WebApi.Data.Entities
{
    public class Cart:BaseEntitiy
    {
        public int ProductID { get; set; }
        public int CustomerID { get; set; }


        public Product Product { get; set; }
        public Customer Customer { get; set; }
    }
}
