using E_Commerce.WebMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http;
using X.PagedList;


namespace E_Commerce.WebMVC.Controllers
{
    public class HomeController : Controller
    {
        readonly private IHttpClientFactory _httpClientFactory;

        public HomeController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }


        public async Task<IActionResult> DisplayIndex(int page = 1)
        {

            List<ProductModel> product = new List<ProductModel>();
            var token = User.Claims.FirstOrDefault(x => x.Type == "accesToken")?.Value;
            if (token != null)
            {
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                var response = await client.GetAsync("http://localhost:5101/api/Product/GetAllProducts");
                if (response.IsSuccessStatusCode)
                {

                    var content = await response.Content.ReadAsStringAsync();
                    product = JsonConvert.DeserializeObject<List<ProductModel>>(content);

                    var pageList = product.ToPagedList(page, 9);

                    return View(pageList);//kontrol et
                }
                else
                {

                    //  hata mesajý gösterme 
                }
            }
            return View(product);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }



        public async Task<IActionResult> Index(int page = 1)
        {

            List<ProductModel> product = new List<ProductModel>();

            {
                var client = _httpClientFactory.CreateClient();

                var response = await client.GetAsync("http://localhost:5101/api/Product/GetAllProducts");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    product = JsonConvert.DeserializeObject<List<ProductModel>>(content);

                    var pageList = product.ToPagedList(page, 9);

                    return View(pageList);//kontrol et
                }
                else
                {

                    //  hata mesajý gösterme 
                }

                return View(product);
            }
        }
    }
}
