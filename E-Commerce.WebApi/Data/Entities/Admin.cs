﻿using E_Commerce.WebApi.Data.Entities.Common;

namespace E_Commerce.WebApi.Data.Entities
{
    public class Admin:BaseEntitiy
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public double PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
