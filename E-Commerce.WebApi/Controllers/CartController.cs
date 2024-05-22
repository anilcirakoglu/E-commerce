using E_Commerce.WebApi.Business.Models;
using E_Commerce.WebApi.Business;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartBO _cartBO;

        public CartController(ICartBO cartBO)
        {
            _cartBO = cartBO;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var customer = _cartBO.GetAll();
            return Ok(customer);
        }
        [HttpPost("Create")]
        public async Task<ActionResult<CartModel>> Create(CartModel cartModel)
        {
            var customer = await _cartBO.Create(cartModel);
            return Ok(customer);
        }
        [HttpGet("{ID}")]
        public async Task<IActionResult> GetByID(int ID)
        {
            var product = await _cartBO.GetByID(ID);
            return Ok(product);
        }
        [HttpDelete("Delete")]
        public async Task<IActionResult> deleteByID(int ID)
        {
            await _cartBO.RemoveAsync(ID);
            return Ok(ID);
        }
        [HttpPut("Update")]
        public async Task<IActionResult> UpdateAsync(CartModel cartModel)
        {
            await _cartBO.UpdateAsync(cartModel);
            return Ok(cartModel);
        }
        [HttpPost("DecreaseCartProduct")]
        public async Task<IActionResult> DecreaseCartProduct([FromBody]int ProductID)
        {
            var cart = _cartBO.DecreaseCartProduct(ProductID);
            return Ok(cart);
        }
        [HttpPost("IncreaseCartProduct")]
        public async Task<IActionResult> IncreaseCartProduct([FromBody] int ProductID)
        {
            var cart =_cartBO.IncreaseCartProduct(ProductID);
            return Ok(cart);
        }

    }
}
