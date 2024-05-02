using E_Commerce.WebApi.Business.Models;
using E_Commerce.WebApi.Business;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SellerController : ControllerBase
    {
        private readonly ISellerBO _sellerBO;

        public SellerController(ISellerBO sellerBO)
        {
            _sellerBO = sellerBO;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var seller = _sellerBO.GetAll();
            return Ok(seller);
        }
        [HttpPost("Create")]
        public async Task<ActionResult<SellerModel>> Create(SellerModel sellerModel)
        {
            var customer = await _sellerBO.Create(sellerModel);
            return Ok(customer);
        }
        [HttpGet("{ID}")]
        public async Task<IActionResult> GetByID(int ID)
        {
            var product = await _sellerBO.GetByID(ID);
            return Ok(product);
        }
        [HttpDelete("Delete")]
        public async Task<IActionResult> deleteByID(int ID)
        {
            await _sellerBO.RemoveAsync(ID);
            return Ok(ID);
        }
        [HttpPut("Update")]
        public async Task<IActionResult> UpdateAsync(SellerModel sellerModel)
        {
            await _sellerBO.UpdateAsync(sellerModel);
            return Ok(sellerModel);
        }
    }
}
