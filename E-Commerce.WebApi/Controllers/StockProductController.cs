using E_Commerce.WebApi.Business.Models;
using E_Commerce.WebApi.Business;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using E_Commerce.WebApi.Business.Enums;
using Microsoft.AspNetCore.Authorization;

namespace E_Commerce.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockProductController : ControllerBase
    {
        private readonly IStockProductBO _stockProductBO;

        public StockProductController(IStockProductBO stockProductBO)
        {
            _stockProductBO = stockProductBO;
        }
       
        [HttpGet]
        public IActionResult GetAll()
        {
            var customer = _stockProductBO.GetAll();
            return Ok(customer);
        }
        [HttpPost("Create")]
        public async Task<ActionResult<StockProductModel>> Create(StockProductModel stockProductModel)
        {
            var customer = await _stockProductBO.Create(stockProductModel);
            return Ok(customer);
        }
        [HttpGet("{ID}")]
        public async Task<IActionResult> GetByID(int ID)
        {
            var product = await _stockProductBO.GetByID(ID);
            return Ok(product);
        }
        [HttpDelete("Delete")]
        public async Task<IActionResult> deleteByID(int ID)
        {
            await _stockProductBO.RemoveAsync(ID);
            return Ok(ID);
        }
        [HttpPut("Update")]
        public async Task<IActionResult> UpdateAsync(StockProductModel stockProductModel)
        {
            await _stockProductBO.UpdateAsync(stockProductModel);
            return Ok(stockProductModel);
        }
      
    }
}
