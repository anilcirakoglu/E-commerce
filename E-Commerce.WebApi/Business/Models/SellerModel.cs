﻿namespace E_Commerce.WebApi.Business.Models
{
    public class SellerModel
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public double PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string CompanyType { get; set; }
        public int TaxpayerIDNumber { get; set; }
        public string Role { get; set; }
    }
}