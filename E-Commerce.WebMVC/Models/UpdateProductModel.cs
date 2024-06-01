using Microsoft.AspNetCore.Mvc.Rendering;

namespace E_Commerce.WebMVC.Models
{
    public class UpdateProductModel
    {
        public int ID { get; set; }
        public string ProductName { get; set; }
        public string ProductInformation { get; set; }
        public double ProductPrice { get; set; }

        public string? Image { get; set; }
        public double DiscountPercentage { get; set; }
        public int CategoryID { get; set; }
        public int ProductQuantity { get; set; }
        public SelectList Categories { get; set; }

        public string CategoryName { get; set; }
        public int SellerID { get; set; }
    }
}
