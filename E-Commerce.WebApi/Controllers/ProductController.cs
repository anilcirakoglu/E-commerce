using E_Commerce.WebApi.Business;
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
        [HttpGet]
        public IActionResult GetAll()
        {
            var product = _productBO.GetAll();
            return Ok(product);
        }
        [HttpPost("Create")]
        public async Task<ActionResult<ProductModel>> Create(ProductModel product)
        {
            var products = await _productBO.Create(product);
            return Ok(products);
        }
        [HttpGet("{ID}")]//hata veriyor olabilir kontrol et data girdikten snra
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
        public async Task<IActionResult> UpdateAsync(ProductModel productModel)
        {
            await _productBO.UpdateAsync(productModel);
            return Ok(productModel);
        }
    }
}
