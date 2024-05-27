namespace E_Commerce.WebApi.Business.Models
{
    public class PurchaseProductModel
    {
        public int ProductID { get; set; }
        public string? ProductName { get; set; }
        public int Quantity { get; set; }
        public double? Price { get; set; }
        public DateTime? PurcaseDate { get; set; }
       
    }
    public class PurcaseProductListModel { 
    
    public List<PurchaseProductModel> PurchasedProducts { get; set; } = new List<PurchaseProductModel>();
    }
}
