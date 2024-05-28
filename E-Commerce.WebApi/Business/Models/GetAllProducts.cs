namespace E_Commerce.WebApi.Business.Models
{
    public class AllProducts
    {

        //Product Name	Information	ProductPrice	DiscountPercentage	Stock Quantity	IsProductActive	Categories

       public int ID { get; set; }
        public string ProductName { get; set; }
        public string ProductInformation { get; set; }
        public double ProductPrice { get; set; }

        public string? Image {  get; set; }
        public double DiscountPercentage { get; set; }
        public bool IsProductActive { get; set; }
        public string CategoryName{ get; set; }
        public string Username { get; set; }
        public int ProductQuantity { get; set; }
        public bool IsApprovedProduct { get; set; } = false;

        public string SellerName { get; set; }
       
    }
}
