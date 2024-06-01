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

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            var token = _sellerBO.Login(loginModel);
            if (token == "")
            {
                return BadRequest();
            }
            else if (token == null) { 
            return StatusCode(403);
            }
            return Ok(token); 
        }
        [HttpPost("Registration")]
        public async Task<IActionResult> Register(SellerDto sellerDto)
        {
            var sellerReg = await _sellerBO.Registration(sellerDto);
            return Ok(sellerReg);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var seller = _sellerBO.GetAll();
            return Ok(seller);
        }
        [HttpPost("Create")]
        public async Task<ActionResult<SellerDto>> Create(SellerDto sellerModel)
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
        public async Task<IActionResult> UpdateAsync(SellerDto sellerModel)
        {
            await _sellerBO.UpdateAsync(sellerModel);
            return Ok(sellerModel);
        }
        [HttpPost("ActiveProduct")]
        public async Task<IActionResult> ActiveProduct([FromBody] int ID)
        {
            await _sellerBO.ActiveProduct(ID); //var product = _sellerBO.ActiveProduct(ID);
            return Ok(ID);

        }
        [HttpPost("PassiveProduct")]
        public async Task<IActionResult> PassiveProduct([FromBody] int ID) 
        {
            await _sellerBO.PassiveProduct(ID);
            return Ok(ID);
        }

    }
}
