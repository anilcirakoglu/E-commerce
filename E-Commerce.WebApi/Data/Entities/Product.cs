using E_Commerce.WebApi.Data.Entities.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commerce.WebApi.Data.Entities
{
    public class Product:BaseEntitiy
    {
        
        public string ProductName { get; set; }
        public string ProductInformation { get; set; }
        public double ProductPrice { get; set; }
        public double previousPrice { get; set; }
        public double discountPercentage {  get; set; }
        public bool IsProductActive { get; set; }
        public string Image { get; set; }
        public int CategoryID {  get; set; }
        public int SellerID { get; set; }

        public bool IsApprovedProduct { get; set; }=false;

       


        public CategoryProduct CategoryProduct { get; set; }
        public Seller Seller { get; set; }
    }
}
