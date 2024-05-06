using Microsoft.AspNetCore.Mvc.Rendering;

namespace E_Commerce.WebMVC.Models
{
    public class ProductModel
    {
        public string ProductName { get; set; }
        public string ProductInformation { get; set; }
        public double ProductPrice { get; set; }
        public double PreviousPrice { get; set; }
        public double DiscountPercentage { get; set; }
        public bool IsProductActive { get; set; }
        public int CategoryID { get; set; }
        public SelectList Categories { get; set; }
        public int SellerID { get; set; }

       
    }
}
