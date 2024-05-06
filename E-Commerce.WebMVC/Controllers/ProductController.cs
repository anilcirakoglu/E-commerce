using E_Commerce.WebMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;

namespace E_Commerce.WebMVC.Controllers
{
    public class ProductController : Controller
    {
        readonly private IHttpClientFactory _httpClientFactory;

        public ProductController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Create()
        {
            List<CategoryProductModel> categoryProducts = new List<CategoryProductModel>();
            var token = User.Claims.FirstOrDefault(x => x.Type == "accesToken")?.Value;
            var product = new ProductModel();
           
            if (token != null)
            {
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                var response = await client.GetAsync("http://localhost:5101/api/CategoryProduct");
                if (response.IsSuccessStatusCode)
                {

                    var content = await response.Content.ReadAsStringAsync();
                   
                    categoryProducts = JsonConvert.DeserializeObject<List<CategoryProductModel>>(content);
                    product.Categories = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(categoryProducts,"ID", "CategoryName");
                }
                else
                {

                    // Örneğin, loglama, hata mesajı gösterme veya başka bir işlem
                }
            }
            

            return View(product);
        }
        [HttpPost]
        public async Task<IActionResult> Create(ProductModel product) 
        {
            if(ModelState.IsValid)
            {

            }
            return View(product);
        }
    }
}
