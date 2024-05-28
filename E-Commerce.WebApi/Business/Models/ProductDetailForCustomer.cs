namespace E_Commerce.WebApi.Business.Models
{
    public class ProductDetailForCustomer
    {
       public int ID { get; set; }
        public string ProductName { get; set; }
        public string ProductInformation { get; set; }
        public double ProductPrice { get; set; }
        public string Username { get; set; }
        public int? ProductQuantity { get; set; }

        public string Img {  get; set; }
    }
}
