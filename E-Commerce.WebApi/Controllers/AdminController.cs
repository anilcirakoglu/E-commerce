using E_Commerce.WebApi.Business.Models;
using E_Commerce.WebApi.Business;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace E_Commerce.WebApi.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    
    public class AdminController : ControllerBase
    {
        private readonly IAdminBO _adminBO;

        public AdminController(IAdminBO adminBO)
        {
            _adminBO = adminBO;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var customer = _adminBO.GetAll();
            return Ok(customer);
        }
        //[HttpPost("Create")]
        //public async Task<ActionResult<AdminModel>> Create(AdminModel adminModel)
        //{
        //    var customer = await _adminBO.Create(adminModel);
        //    return Ok(customer);
        //}
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            var token = _adminBO.Login(loginModel);
            if (token == "")
            {
                return BadRequest();
            }
            return Ok(token);
        }
        [HttpPost("Registration")]
        public async Task<IActionResult> Register(AdminDto adminDto)
        {
            var adminReg = await _adminBO.Registration(adminDto);
            return Ok(adminReg);
        }
        [HttpGet("{ID}")]
        public async Task<IActionResult> GetByID(int ID)
        {
            var product = await _adminBO.GetByID(ID);
            return Ok(product);
        }
        [HttpDelete("Delete")]
        public async Task<IActionResult> deleteByID(int ID)
        {
            await _adminBO.RemoveAsync(ID);
            return Ok(ID);
        }
        [HttpPut("Update")]
        public async Task<IActionResult> UpdateAsync(AdminDto adminModel)
        {
            await _adminBO.UpdateAsync(adminModel);
            return Ok(adminModel);
        }
       
        [HttpPost("ApprovedSeller")]
        public async Task<IActionResult> ApprovedSeller( [FromBody]int  ID)
        {
            await _adminBO.ApprovedSeller(ID);
            return Ok(ID);
        }
        [HttpPost("RejectSeller")]
        public async Task<IActionResult> RejectSeller([FromBody] int ID)
        {
            await _adminBO.RejectSeller(ID);
            return Ok(ID);
        }
    }
}
