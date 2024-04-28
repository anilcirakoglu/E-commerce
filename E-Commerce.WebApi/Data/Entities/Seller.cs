using E_Commerce.WebApi.Data.Entities.Common;

namespace E_Commerce.WebApi.Data.Entities
{
    public class Seller : BaseEntitiy
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string CompanyType { get; set; }
        public int TaxpayerIDNumber { get; set; }
        public string Role { get; set; }

    }
}
