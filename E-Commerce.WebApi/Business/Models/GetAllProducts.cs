namespace E_Commerce.WebApi.Business.Models
{
    public class GetAllProducts
    {

        //Product Name	Information	ProductPrice	DiscountPercentage	Stock Quantity	IsProductActive	Categories


        public string ProductName { get; set; }
        public string ProductInformation { get; set; }
        public double ProductPrice { get; set; }

       
        public double DiscountPercentage { get; set; }
        public bool IsProductActive { get; set; }
        public string CategoryName{ get; set; }
        public int ProductQuantity { get; set; }

        
       
    }
}
