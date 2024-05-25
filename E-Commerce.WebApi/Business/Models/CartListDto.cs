using System.Numerics;

namespace E_Commerce.WebApi.Business.Models
{
    public class CartListDto
    {
        public int ProductID { get; set; }
        public string? ProductName { get; set; }
        public int Quantity { get; set; }
        public double? Price { get; set; }
        public double? TotalPrice { get; set; }
    }
}
