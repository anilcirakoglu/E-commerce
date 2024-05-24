using E_Commerce.WebMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json;
using X.PagedList;

namespace E_Commerce.WebMVC.Controllers
{
    
    public class CategoryProductController : Controller
    {
        readonly private IHttpClientFactory _httpClientFactory;

        public CategoryProductController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
      
        [AllowAnonymous]
        public IActionResult AccessDenied() { 
        return View();
        }

        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> Category(int page=1)
        {
            List<CategoryProductModel> categoryProducts = new List<CategoryProductModel>();


            var token = User.Claims.FirstOrDefault(x => x.Type == "accesToken")?.Value;
            if (token != null)
            {
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                var response = await client.GetAsync("http://localhost:5101/api/CategoryProduct");
                if (response.IsSuccessStatusCode)
                {

                    var content = await response.Content.ReadAsStringAsync();
                    categoryProducts = JsonConvert.DeserializeObject<List<CategoryProductModel>>(content);
                    var pageList = categoryProducts.ToPagedList(page, 5);
                    return View(pageList);

                }
               
            }
            return View(categoryProducts);
        }
        [HttpGet]
        public async Task<IActionResult> CategoryList(int ID, int page = 1)
        {
            List<ProductForCustomerModel> product = new List<ProductForCustomerModel>();
            var token = User.Claims.FirstOrDefault(x => x.Type == "accesToken")?.Value;
            var jwtId = User.Claims.FirstOrDefault(claim => claim.Type == "nameid")?.Value;
            if (jwtId == null)
            {
                return RedirectToAction("Index", "Login");
            }
            if (token != null)
            {
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                var response = await client.GetAsync($"http://localhost:5101/api/CategoryProduct/CategoryList/{ID}");
                if (response.IsSuccessStatusCode)
                {

                    var content = await response.Content.ReadAsStringAsync();
                    product = JsonConvert.DeserializeObject<List<ProductForCustomerModel>>(content);


                    var pageList = product.ToPagedList(page, 9);
                    return View(pageList);
                }

            }
            return View(product);


        }

        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> Edit(CategoryProductModel categoryProduct)
        {
            if (ModelState.IsValid)
            {
                var token = User.Claims.FirstOrDefault(x => x.Type == "accesToken")?.Value;
                var jwtId = User.Claims.FirstOrDefault(claim => claim.Type == "nameid")?.Value;
         



                if (token != null)
                {
                    var client = _httpClientFactory.CreateClient();
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);


                    var data = JsonConvert.SerializeObject(categoryProduct);
                    var content = new StringContent(data, Encoding.UTF8, "application/json");
                    var response = await client.PutAsync($"http://localhost:5101/api/CategoryProduct/Update", content);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Category", "CategoryProduct");
                    }
                    ModelState.AddModelError("", "wrong Model");

                }
            }
            return View(categoryProduct);
        }
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> Create(CategoryProductModel categoryproductModel)
        {
            var token = User.Claims.FirstOrDefault(x => x.Type == "accesToken")?.Value;
            var jwtId = User.Claims.FirstOrDefault(claim => claim.Type == "nameid")?.Value;

            if (token != null)
            {


                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);


                var data = JsonConvert.SerializeObject(categoryproductModel);
                var content = new StringContent(data, Encoding.UTF8, "application/json");
                var response = await client.PostAsync($"http://localhost:5101/api/CategoryProduct/Create", content);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Category", "CategoryProduct");
                }
                ModelState.AddModelError("", "wrong Model");
            }


            return View(categoryproductModel);
        }
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> Remove(int ID) 
        {
            var token = User.Claims.FirstOrDefault(x => x.Type == "accesToken")?.Value;
         
            if (token != null)
            {
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await client.DeleteAsync($"http://localhost:5101/api/CategoryProduct/Delete/{ID}");
                

            }

            return RedirectToAction("Category", "CategoryProduct");
        }
    }






}
