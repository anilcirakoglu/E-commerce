using E_Commerce.WebMVC.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using System.Net.Mime;
using Microsoft.AspNetCore.Http;

namespace E_Commerce.WebMVC.Controllers
{
    public class SellerController : Controller
    {
        readonly private IHttpClientFactory _httpClientFactory;

        public SellerController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public IActionResult Login()
        {
            return View(new LoginModel());
        }
        public IActionResult SignIn()
        {
            return View(new SellerModel());
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        #region loginANDlogout
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var client = this._httpClientFactory.CreateClient();
                var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

                var response = await client.PostAsync("http://localhost:5101/api/Seller/Login", content);

                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    if (data != null)
                    {
                        JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                        var token = handler.ReadJwtToken(data);

                        var claims = token.Claims.ToList();
                        if (data != null)
                            claims.Add(new Claim("accesToken", data));

                        var claimsIdentity = new ClaimsIdentity(claims, JwtBearerDefaults.AuthenticationScheme);

                        await HttpContext.SignInAsync(JwtBearerDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));


                    }
                    return RedirectToAction("Index", "Home");
                }
               
                else
                {
                    ModelState.AddModelError("Password", "Your password is incorrect, Please enter again");
                    ModelState.AddModelError("Email", "Your Email is incorrect, Please enter again");
                    
                }



                return View(model);

            }
            return View(model);
        }



        #endregion
        [HttpPost]
        public async Task<IActionResult> SignIn(SellerModel seller)
        {

            var token = User.Claims.FirstOrDefault(x => x.Type == "accesToken")?.Value;


            if (token == null)
            {
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var data = JsonConvert.SerializeObject(seller);
                var content = new StringContent(data, Encoding.UTF8, "application/json");

                var response = await client.PostAsync("http://localhost:5101/api/Seller/Registration", content);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Login", "Seller");
                }
                ModelState.AddModelError("", "wrong Model");

            }


            return View(seller);
        }
        [Authorize(Policy = ("SellerPolicy"))]
        public async Task<IActionResult> Edit()
        {
            var seller = new SellerModel();
            if (ModelState.IsValid)
            {
                var token = User.Claims.FirstOrDefault(x => x.Type == "accesToken")?.Value;

                var jwtId = User.Claims.FirstOrDefault(claim => claim.Type == "nameid")?.Value;
                int jwtID = int.Parse(jwtId);
                
                int ID = jwtID;//seller.ID = jwtID;

                if (token != null)
                {
                    var client = _httpClientFactory.CreateClient();
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                    var response = await client.GetAsync($"http://localhost:5101/api/Seller/{ID}");


                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();

                        var sellerInfo = JsonConvert.DeserializeObject<SellerModel>(content);

                        return View(sellerInfo);
                    }
                    else
                    {
                        ModelState.AddModelError("", "wrong Model");
                    }


                }

            }
            return View(seller);
        }


        /*------------------------------------------------------*/
        [Authorize(Policy = ("SellerPolicy"))]
        [HttpPost]
        public async Task<IActionResult> Edit(SellerModel seller)
        {

            if (ModelState.IsValid)
            {
                var token = User.Claims.FirstOrDefault(x => x.Type == "accesToken")?.Value;

                var jwtId = User.Claims.FirstOrDefault(claim => claim.Type == "nameid")?.Value;
                int jwtID = int.Parse(jwtId);
                seller.ID = jwtID;

                if (token != null)
                {

                    var client = _httpClientFactory.CreateClient();
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);


                    var data = JsonConvert.SerializeObject(seller);



                    var content = new StringContent(data, Encoding.UTF8, "application/json");

                    var response = await client.PutAsync($"http://localhost:5101/api/Seller/Update", content);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "wrong Model");
                    }


                }

            }
            return View(seller);
        }

        public async Task<IActionResult> EditProduct(int ID)
        {
            

            var productModel = new ProductModel();

            var token = User.Claims.FirstOrDefault(x => x.Type == "accesToken")?.Value;
            var jwtId = User.Claims.FirstOrDefault(claim => claim.Type == "nameid")?.Value;
            int jwtID = int.Parse(jwtId);

            productModel.ID = ID;

         
            if (token != null)
            {
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                var response = await client.GetAsync($"http://localhost:5101/api/Product/{ID}");
                var response2 = await client.GetAsync("http://localhost:5101/api/CategoryProduct");

                if (response.IsSuccessStatusCode && response2.IsSuccessStatusCode)
                {

                    var content = await response.Content.ReadAsStringAsync();
                    var content2 = await response2.Content.ReadAsStringAsync();


                    



                    var categoryProducts = JsonConvert.DeserializeObject<List<CategoryProductModel>>(content2);
                    var productInfo = JsonConvert.DeserializeObject<UpdateProductModel>(content);
                    byte[] bytesImage = Convert.FromBase64String(productInfo.Image);

                    

               //     using (var stream = new MemoryStream(bytesImage))
               //{

                    //    var formFile = new FormFile(stream, 0, stream.Length, null, "image")
                    //    {
                    //        Headers = new HeaderDictionary(),

                    //    };


                    //}


                    //productModel.ProductName = productInfo.ProductName;
                    //productModel.ProductPrice = productInfo.ProductPrice;
                    //productModel.ProductQuantity= productInfo.ProductQuantity;
                    //productModel.DiscountPercentage = productInfo.DiscountPercentage;
                    //productModel.ProductInformation = productInfo.ProductInformation;
                    //productModel.Image = 



                    //productInfo.Categories = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(categoryProducts, "CategoryID", "CategoryName");

                    return View(productInfo);
                }

            }
            return RedirectToAction("SellerProductList");
        }
        [Authorize(Policy = ("SellerPolicy"))]
        [HttpPost]
        public async Task<IActionResult> EditProduct(ProductModel productModel)
        {


            var token = User.Claims.FirstOrDefault(x => x.Type == "accesToken")?.Value;

            var jwtId = User.Claims.FirstOrDefault(claim => claim.Type == "nameid")?.Value;
            int jwtID = int.Parse(jwtId);


            if (token != null)
            {
                UpdateProductModel updateProductModel = new UpdateProductModel();
                byte[] bytes;

                using (BinaryReader binaryReader = new BinaryReader(productModel.Image.OpenReadStream()))
                {
                    bytes = binaryReader.ReadBytes((int)productModel.Image.OpenReadStream().Length);
                }

                string base64ImageRepresentation = Convert.ToBase64String(bytes);

                updateProductModel.SellerID = jwtID;
                updateProductModel.ID = productModel.ID;
                updateProductModel.ProductName = productModel.ProductName;
                updateProductModel.ProductPrice = productModel.ProductPrice;
                updateProductModel.ProductQuantity = productModel.ProductQuantity;
                updateProductModel.DiscountPercentage = productModel.DiscountPercentage;
                updateProductModel.ProductInformation = productModel.ProductInformation;
                updateProductModel.Image = base64ImageRepresentation;
                updateProductModel.CategoryID = productModel.CategoryID;
                updateProductModel.CategoryName = productModel.CategoryName;




                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);


                var data = JsonConvert.SerializeObject(updateProductModel);
                var content = new StringContent(data, Encoding.UTF8, "application/json");

                var response = await client.PutAsync($"http://localhost:5101/api/Product/Update", content);
                if (response.IsSuccessStatusCode)
                {

                    return RedirectToAction("SellerProductList", "Product");
                }




            }
            return View(productModel);
        }
        [Authorize(Policy = "SellerPolicy")]
        public async Task<IActionResult> ActiveProduct(int ID)
        {

            var token = User.Claims.FirstOrDefault(x => x.Type == "accesToken")?.Value;
            if (token != null)
            {
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var data = JsonConvert.SerializeObject(ID);
                var content = new StringContent(data, Encoding.UTF8, "application/json");
                var response = await client.PostAsync($"http://localhost:5101/api/Seller/ActiveProduct", content);
                if (response.IsSuccessStatusCode)
                {

                    var contents = await response.Content.ReadAsStringAsync();
                    ID = JsonConvert.DeserializeObject<int>(contents);
                }

            }

            return RedirectToAction("SellerProductList", "Product");
        }
        public async Task<IActionResult> PassiveProduct(int ID)
        {

            var token = User.Claims.FirstOrDefault(x => x.Type == "accesToken")?.Value;
            if (token != null)
            {
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var data = JsonConvert.SerializeObject(ID);
                var content = new StringContent(data, Encoding.UTF8, "application/json");
                var response = await client.PostAsync($"http://localhost:5101/api/Seller/PassiveProduct", content);
                if (response.IsSuccessStatusCode)
                {

                    var contents = await response.Content.ReadAsStringAsync();
                    ID = JsonConvert.DeserializeObject<int>(contents);
                }

            }

            return RedirectToAction("SellerProductList", "Product");
        }



    }



}


