namespace E_Commerce.WebMVC.Models
{
    public class ProductForCustomerModel
    {
        public int ID { get; set; }
        public string ProductName { get; set; }
        public string ProductInformation { get; set; }
        public double ProductPrice { get; set; }

        public string? Image { get; set; }
        public double DiscountPercentage { get; set; }
        public bool IsProductActive { get; set; }
        public int CategoryID { get; set; }
        public int ProductQuantity { get; set; }

        public string CategoryName { get; set; }

        public bool IsApprovedProduct { get; set; } = false;
        public int SellerID { get; set; }
    }
}
