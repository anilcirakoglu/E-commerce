namespace E_Commerce.WebMVC.Models
{
    public class SellerModel
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Password { get; set; }
        public double PhoneNumber { get; set; }
        public string Username { get; set; }
        public bool IsApprove { get; set; }=false;
        public string CompanyType { get; set; }
        public int TaxpayerIDNumber { get; set; }

    }
}
