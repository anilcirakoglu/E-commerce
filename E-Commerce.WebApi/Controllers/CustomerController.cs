using E_Commerce.WebApi.Business.Models;
using E_Commerce.WebApi.Business;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerBO _customerBO;

        public CustomerController(ICustomerBO customerBO)
        {
            _customerBO = customerBO;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var customer = _customerBO.GetAll();
            return Ok(customer);
        }
        [HttpPost("Create")]
        public async Task<ActionResult<CustomerModel>> Create(CustomerModel customerModel)
        {
            var customer = await _customerBO.Create(customerModel);
            return Ok(customer);
        }
        [HttpGet("{ID}")]
        public async Task<IActionResult> GetByID(int ID)
        {
            var customer = await _customerBO.GetByID(ID);
            return Ok(customer);
        }
        [HttpDelete("Delete")]
        public async Task<IActionResult> deleteByID(int ID)
        {
            await _customerBO.RemoveAsync(ID);
            return Ok(ID);
        }
        [HttpPut("Update")]
        public async Task<IActionResult> UpdateAsync(CustomerModel customerModel)
        {
            await _customerBO.UpdateAsync(customerModel);
            return Ok(customerModel);
        }
    }
}
