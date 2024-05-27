namespace E_Commerce.WebMVC.Models
{
    public class PurchaseProduct
    {
        public int ProductID { get; set; }
        public string? ProductName { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public DateTime? PurcaseDate { get; set; }
    }
}
