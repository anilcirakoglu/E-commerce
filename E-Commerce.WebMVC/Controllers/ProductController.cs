using E_Commerce.WebMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Text;
using X.PagedList;

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
            var product = new ProductModel();
            var token = User.Claims.FirstOrDefault(x => x.Type == "accesToken")?.Value;


            if (token != null)
            {
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                var response = await client.GetAsync("http://localhost:5101/api/CategoryProduct");

                if (response.IsSuccessStatusCode)
                {

                    var content = await response.Content.ReadAsStringAsync();

                    var categoryProducts = JsonConvert.DeserializeObject<List<CategoryProductModel>>(content);
                    product.Categories = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(categoryProducts, "ID", "CategoryName");

                    return View(product);
                }
                else
                {

                    // hata mesajı gösterme 
                }
            }


            return RedirectToAction("ListProduct");
        }
        [HttpPost]
        public async Task<IActionResult> Create(ProductModel productModel)
        {
            var token = User.Claims.FirstOrDefault(x => x.Type == "accesToken")?.Value;
            var jwtId = User.Claims.FirstOrDefault(claim => claim.Type == "nameid")?.Value;
            int jwtID = int.Parse(jwtId);
            productModel.SellerID = jwtID;




            if (token != null)
            {


                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);


                var data = JsonConvert.SerializeObject(productModel);
                var content = new StringContent(data, Encoding.UTF8, "application/json");



                var response = await client.PostAsync($"http://localhost:5101/api/Product/Create", content);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("ListProduct", "Product");
                }
                ModelState.AddModelError("", "wrong Model");
            }


            return View(productModel);
        }

        [HttpGet]
        public async Task<IActionResult> ListProduct()//admin ile seller ayrıldı mı kontrol et
        {
            List<ProductModel> product = new List<ProductModel>();
            var token = User.Claims.FirstOrDefault(x => x.Type == "accesToken")?.Value;
            if (token != null)
            {
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                var response = await client.GetAsync("http://localhost:5101/api/Product/GetAllProductsForAdmin");
                if (response.IsSuccessStatusCode)
                {

                    var content = await response.Content.ReadAsStringAsync();
                    product = JsonConvert.DeserializeObject<List<ProductModel>>(content);

                 
                }
               
            }
            return View("ProductList", product);
        }

        [Authorize(Policy = "SellerPolicy")]
        public async Task<IActionResult> SellerProductList()
        {
            List<ProductModel> product = new List<ProductModel>();
            var token = User.Claims.FirstOrDefault(x => x.Type == "accesToken")?.Value;
            var jwtId = User.Claims.FirstOrDefault(claim => claim.Type == "nameid")?.Value;
            var role = User.Claims.FirstOrDefault(claim => claim.Type == "role")?.Value;

            int jwtID = int.Parse(jwtId);

            if (token != null)
            {
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);



                var response = await client.GetAsync($"http://localhost:5101/api/Product/SellerProducts/{jwtID}");
                if (response.IsSuccessStatusCode)
                {

                    var content = await response.Content.ReadAsStringAsync();
                    product = JsonConvert.DeserializeObject<List<ProductModel>>(content);
                }
                else
                {

                    //  hata mesajı gösterme 
                }
            }
            return View("SellerProductList", product);
        }
        [HttpGet]
        public async Task<IActionResult> Search(string name, int page=1) //yoksa ürün boş sayfa geliyor düzelt
        {
            List<ProductModel> product = new List<ProductModel>();
            var token = User.Claims.FirstOrDefault(x => x.Type == "accesToken")?.Value;
            if (token != null)
            {
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                var response = await client.GetAsync($"http://localhost:5101/api/Product/Search/{name}");
                if (response.IsSuccessStatusCode)
                {

                    var content = await response.Content.ReadAsStringAsync();
                    product = JsonConvert.DeserializeObject<List<ProductModel>>(content);

                    var pageList = product.ToPagedList(page, 9);
                    return View(pageList);
                }
               
            }
            return View(product);
        }


        [HttpGet]
        public async Task<IActionResult> Details(int ID)
        {
           
            var token = User.Claims.FirstOrDefault(x => x.Type == "accesToken")?.Value;
            var jwtId = User.Claims.FirstOrDefault(claim => claim.Type == "nameid")?.Value;
            if(jwtId==null)
            {
                return RedirectToAction("Index", "Login");
            }
            var product = new ProductDetailForCustomerModel();
            if (token != null)
            {
                
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                var response = await client.GetAsync($"http://localhost:5101/api/Product/Details/{ID}");
                if (response.IsSuccessStatusCode)
                {

                    var content = await response.Content.ReadAsStringAsync();
                    product = JsonConvert.DeserializeObject<ProductDetailForCustomerModel>(content);
                    return View(product);
                  
                }

            }
            return RedirectToAction("Details", "Product");
        }




    }
}
