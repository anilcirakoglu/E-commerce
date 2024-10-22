﻿using E_Commerce.WebApi.Business;
using E_Commerce.WebApi.Business.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        readonly private IProductBO _productBO;
        public ProductController(IProductBO productBO)
        {
            _productBO = productBO;
        }
        [HttpGet("GetAllProductsForAdmin")]
        public ActionResult<AllProducts> GetAllProductsForAdmin()
        {
            var product = _productBO.GetAllProductsForAdmin();
            return Ok(product);
        }

        [HttpGet("GetAllProducts")]
        public  ActionResult<AllProducts> GetAllProducts()
        {
            var product = _productBO.GetAllProducts();
            return Ok(product);
        }
        [HttpGet("SellerProducts/{ID:int}")]
        public ActionResult<ProductDto> SellerProducts([FromRoute] int ID)
        {
            var product = _productBO.sellerProducts(ID);
            return Ok(product);
        }
        [HttpGet("Details/{ID}")]
        public async Task<ActionResult<ProductDetailForCustomer>> DetailForCustomer(int ID) {
            try {
                var product = await _productBO.DetailForCustomer(ID);
                return Ok(product);
            }
            catch (Exception ex) {
            return BadRequest(ex.Message);
            }
       
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var product = _productBO.GetAll();
            return Ok(product);
        }
        [HttpPost("Create")]
        public async Task<ActionResult<ProductDto>> Create([FromBody]ProductDto product)
        {
            var products = await _productBO.Create(product);
            return Ok(products);
        }
        [HttpGet("{ID}")]
        public async Task<IActionResult> GetByID(int ID)
        {
            var product = await _productBO.GetByID(ID);
            return Ok(product);
        }
        [HttpDelete("Delete")]
        public async Task<IActionResult> deleteByID(int ID)
        {
            await _productBO.RemoveAsync(ID);
            return Ok(ID);
        }
        [HttpPut("Update")]
        public async Task<IActionResult> UpdateAsync(ProductDto product)
        {
            await _productBO.UpdateAsync(product);
            return Ok(product);
        }
        [HttpGet("Search/{name}")]
        public async Task<IActionResult> Search(string name) 
        {
            var product = _productBO.Search(name);
            return Ok(product);
        }
       
    }
}
