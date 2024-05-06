using E_Commerce.WebMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json;

namespace E_Commerce.WebMVC.Controllers
{
    public class CategoryProductController : Controller
    {
        readonly private IHttpClientFactory _httpClientFactory;

        public CategoryProductController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        
        public async Task<IActionResult> Category()
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
                }
                else
                {
                   
                    // Örneğin, loglama, hata mesajı gösterme veya başka bir işlem
                }
            }





            return View(categoryProducts);

        }
    }
}
