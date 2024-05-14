using E_Commerce.WebApi.Business.Models;
using AutoMapper;

namespace E_Commerce.WebApi.Business.Mappings
{
    public class AutoMapping:Profile
    {
        public AutoMapping()
        {
            CreateMap<CustomerModel, CustomerDto>();
            CreateMap<SellerModel, SellerDto>();
            CreateMap<AdminModel,AdminDto>();
            CreateMap<ProductModel, ProductDto>();
            CreateMap<CartModel, CartDto>();
            CreateMap<ProductModel, ProductDetailForCustomer>();
        }
    }
}
