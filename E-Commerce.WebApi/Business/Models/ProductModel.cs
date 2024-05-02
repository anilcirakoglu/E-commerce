namespace E_Commerce.WebApi.Business.Models
{
    public class ProductModel
    {
        public int ID { get; set; }
        public string ProductName { get; set; }
        public string ProductInformation { get; set; }
        public double ProductPrice { get; set; }
        public double PreviousPrice { get; set; }
        public double DiscountPercentage { get; set; }
        public bool IsProductActive { get; set; }
        public int CategoryID { get; set; }
        public int SellerID { get; set; }
    }
}
