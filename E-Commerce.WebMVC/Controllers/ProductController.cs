using E_Commerce.WebMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Text;
using X.PagedList;
using static System.Net.Mime.MediaTypeNames;

namespace E_Commerce.WebMVC.Controllers
{
    /// <summary>
    /// https://learn.microsoft.com/en-us/aspnet/core/fundamentals/http-requests?view=aspnetcore-8.0 (IHttpClientFactory use)
    /// </summary>
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

            }


            return RedirectToAction("ListProduct");
        }
        /// <summary>
        /// https://www.c-sharpcorner.com/article/upload-single-or-multiple-files-in-asp-net-core-using-iformfile2/ (create file operation)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create(ProductModel productModel)
        {

            var token = User.Claims.FirstOrDefault(x => x.Type == "accesToken")?.Value;
            var jwtId = User.Claims.FirstOrDefault(claim => claim.Type == "nameid")?.Value;
            int jwtID = int.Parse(jwtId);




            if (token != null)
            {
                CreateProductModel createProductModel = new CreateProductModel();
                byte[] bytes;

                using (BinaryReader binaryReader = new BinaryReader(productModel.Image.OpenReadStream()))
                {
                    bytes = binaryReader.ReadBytes((int)productModel.Image.OpenReadStream().Length);
                }

                string base64ImageRepresentation = Convert.ToBase64String(bytes);

                createProductModel.SellerID = jwtID;

                createProductModel.ProductName = productModel.ProductName;
                createProductModel.ProductPrice = productModel.ProductPrice;
                createProductModel.ProductQuantity = productModel.ProductQuantity;
                createProductModel.DiscountPercentage = productModel.DiscountPercentage;
                createProductModel.ProductInformation = productModel.ProductInformation;
                createProductModel.IsApprovedProduct = productModel.IsApprovedProduct;
                createProductModel.Image = base64ImageRepresentation;
                createProductModel.CategoryName = productModel.CategoryName;
                createProductModel.IsProductActive = productModel.IsProductActive;
                createProductModel.CategoryID = productModel.CategoryID;





                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);


                var data = JsonConvert.SerializeObject(createProductModel);
                var content = new StringContent(data, Encoding.UTF8, "application/json");



                var response = await client.PostAsync($"http://localhost:5101/api/Product/Create", content);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("SellerProductList", "Product");
                }
                ModelState.AddModelError("", "wrong Model");
            }


            return View(productModel);
        }
        [Authorize(Policy = "AdminPolicy")]
        [HttpGet]
        public async Task<IActionResult> ListProduct(int page = 1)
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
                    var pageList = product.ToPagedList(page, 5);
                    return View(pageList);

                }

            }
            return View("ListProduct", product);
        }

        [Authorize(Policy = "SellerPolicy")]
        public async Task<IActionResult> SellerProductList(int page = 1)
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

                    var pageList = product.ToPagedList(page, 5);
                    return View(pageList);
                }
                else
                {

                    //  hata mesajı gösterme 
                }
            }
            return View("SellerProductList", product);
        }



        [HttpGet]
        public async Task<IActionResult> Details(int ID)
        {
            
                var token = User.Claims.FirstOrDefault(x => x.Type == "accesToken")?.Value;
                var jwtId = User.Claims.FirstOrDefault(claim => claim.Type == "nameid")?.Value;
                if (jwtId == null)
                {
                    return RedirectToAction("Login", "Customer");
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

                    }else
                    {
                        var contents = await response.Content.ReadAsStringAsync();
                        return View("ProductErrorPage", contents);
                    }

                }
                return RedirectToAction("Details", "Product");
            
           
        }
        [HttpGet]
        public async Task<IActionResult> Search(string name, int page = 1)
        {
            List<ProductForCustomerModel> product = new List<ProductForCustomerModel>();
            var token = User.Claims.FirstOrDefault(x => x.Type == "accesToken")?.Value;
            var jwtId = User.Claims.FirstOrDefault(claim => claim.Type == "nameid")?.Value;
            if (jwtId == null)
            {
                return RedirectToAction("Login", "Customer");
            }
            if (ModelState.IsValid)
            {
            
                if (token != null)
                {
                    var client = _httpClientFactory.CreateClient();
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                    var response = await client.GetAsync($"http://localhost:5101/api/Product/Search/{name}");
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
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }




}
