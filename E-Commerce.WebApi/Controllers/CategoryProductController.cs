﻿using E_Commerce.WebApi.Business.Models;
using E_Commerce.WebApi.Business;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryProductController : ControllerBase
    {
       readonly private ICategoryProductBO _categoryProductBO;

        public CategoryProductController(ICategoryProductBO categoryProductBO)
        {
            _categoryProductBO = categoryProductBO;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var categoryProduct = _categoryProductBO.GetAll();
            return Ok(categoryProduct);
        }
        [HttpPost("Create")]
        public async Task<ActionResult<CategoryProductModel>> Create(CategoryProductModel categoryProductModel)
        {
            var categoryProduct = await _categoryProductBO.Create(categoryProductModel);
            return Ok(categoryProduct);
        }
        [HttpGet("{ID}")]
        public async Task<IActionResult> GetByID(int ID)
        {
            var product = await _categoryProductBO.GetByID(ID);
            return Ok(product);
        }
        [HttpDelete("Delete")]
        public async Task<IActionResult> deleteByID(int ID)
        {
            await _categoryProductBO.RemoveAsync(ID);
            return Ok(ID);
        }
        [HttpPut("Update")]
        public async Task<IActionResult> UpdateAsync(CategoryProductModel categoryProductModel)
        {
            await _categoryProductBO.UpdateAsync(categoryProductModel);
            return Ok(categoryProductModel);
        }
    }
}