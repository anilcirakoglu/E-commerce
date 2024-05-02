using E_Commerce.WebApi.Business.Models;
using E_Commerce.WebApi.Business;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

namespace E_Commerce.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerBO _customerBO;
        readonly private IMapper _mapper;

        public CustomerController(ICustomerBO customerBO,IMapper mapper)
        {
            _customerBO = customerBO;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var customer = _customerBO.GetAll();
            var customerDto = _mapper.Map<List<CustomerDto>>(customer);
            return Ok(customerDto);
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
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            var customer = _customerBO.Login(loginModel);
            return Ok(customer);
        }
        [HttpPost("Registration")]
        public async Task<IActionResult>Register(CustomerDto customerDto)
        {
            var cusReg =await _customerBO.Registration(customerDto);
            return Ok(cusReg);
        }
    }
}
