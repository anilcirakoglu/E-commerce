using E_Commerce.WebMVC.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace E_Commerce.WebMVC.Controllers
{
    public class CustomerController : Controller
    {
        readonly private IHttpClientFactory _httpClientFactory;

        public CustomerController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public IActionResult Login()
        {
            return View(new LoginModel());
        }
        public IActionResult SignIn()
        {
            return View(new CustomerModel());
        }
       
        public IActionResult CreditCart()
        {
            return View();
        }
        public IActionResult Index() 
        {
            return View();
        }


        [HttpPost]
        #region login
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var client = this._httpClientFactory.CreateClient();
                var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

                var response = await client.PostAsync("http://localhost:5101/api/Customer/Login", content);

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
                    return RedirectToAction("Index", "Home");//index gönderince hata veriyordu düzeldi ama bir daha kontrol et
                }

                else { 
                    ModelState.AddModelError("Password", "Your password is incorrect, Please enter again");
                    ModelState.AddModelError("Email", "Your Email is incorrect, Please enter again");
                }
                
                return View(model);

            }
            return View(model);
        }

        #endregion
        [HttpPost]
        public async Task<IActionResult> SignIn(CustomerModel customer)
        {
            if (ModelState.IsValid)
            {
                var token = User.Claims.FirstOrDefault(x => x.Type == "accesToken")?.Value;


                if (token == null)
                {
                    var client = _httpClientFactory.CreateClient();
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                    var data = JsonConvert.SerializeObject(customer);
                    var content = new StringContent(data, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync("http://localhost:5101/api/Customer/Registration", content);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Login", "Customer");
                    }
                    ModelState.AddModelError("", "wrong Model");

                }

            }
            return View(customer);
        }

        public async Task<IActionResult> Edit()
        {
            var customer = new CustomerModel();
            if (ModelState.IsValid)
            {
                var token = User.Claims.FirstOrDefault(x => x.Type == "accesToken")?.Value;

                var jwtId = User.Claims.FirstOrDefault(claim => claim.Type == "nameid")?.Value;
                int jwtID = int.Parse(jwtId);
                int ID = jwtID;

                if (token != null)
                {

                    var client = _httpClientFactory.CreateClient();
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                    var response = await client.GetAsync($"http://localhost:5101/api/Customer/{ID}");


                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();

                        var CustomerInfo = JsonConvert.DeserializeObject<CustomerModel>(content);

                        return View(CustomerInfo);
                    }
                    else
                    {
                        ModelState.AddModelError("", "wrong Model");
                    }
                }
            }
            return View(customer);
        }



        /*-------------------------------------------------------------------------------------*/
        [Authorize(Policy ="CustomerPolicy")]
        [HttpPost]
        public async Task<IActionResult> Edit(CustomerModel customer)
        {
            if (ModelState.IsValid)
            {
                var token = User.Claims.FirstOrDefault(x => x.Type == "accesToken")?.Value;

                var jwtId = User.Claims.FirstOrDefault(claim => claim.Type == "nameid")?.Value;
                int jwtID = int.Parse(jwtId);
                customer.ID = jwtID;

                if (token != null)
                {

                    var client = _httpClientFactory.CreateClient();
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);


                    var data = JsonConvert.SerializeObject(customer);



                    var content = new StringContent(data, Encoding.UTF8, "application/json");

                    var response = await client.PutAsync($"http://localhost:5101/api/Customer/Update", content);
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
            return View(customer);
        }
       
        [Authorize(Policy ="CustomerPolicy")]
        [HttpPost]
        public async Task<IActionResult> Purchase()
        {
            
            var token = User.Claims.FirstOrDefault(x => x.Type == "accesToken")?.Value;
            if (token != null)
            {
                var jwtId = User.Claims.FirstOrDefault(claim => claim.Type == "nameid")?.Value;
                int jwtID = int.Parse(jwtId);

                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var data = JsonConvert.SerializeObject(jwtID);
                var content = new StringContent(data, Encoding.UTF8, "application/json");
                var response = await client.PostAsync($"http://localhost:5101/api/Customer/Purchase", content);
                if (response.IsSuccessStatusCode)
                {
                    var contents = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<bool>(contents);
                }
                else
                {
                    var contents = await response.Content.ReadAsStringAsync();
                    return View("ErrorPage",contents);
                }
            }    
            return RedirectToAction("Index", "Home");
        }




        [Authorize(Policy ="CustomerPolicy")]
        [HttpPost]
        public async Task<IActionResult> DecreaseCart(CartModel cart)
        {

            var token = User.Claims.FirstOrDefault(x => x.Type == "accesToken")?.Value;
            if (token != null)
            {
                var jwtId = User.Claims.FirstOrDefault(claim => claim.Type == "nameid")?.Value;
                int jwtID = int.Parse(jwtId);

                cart.CustomerID = jwtID;
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var data = JsonConvert.SerializeObject(cart);
                var content = new StringContent(data, Encoding.UTF8, "application/json");
                var response = await client.PostAsync($"http://localhost:5101/api/Cart/DecreaseCartProduct", content);
                if (response.IsSuccessStatusCode)
                {
                    var contents = await response.Content.ReadAsStringAsync();
                    cart = JsonConvert.DeserializeObject<CartModel>(contents);
                }
            }
                return RedirectToAction("CartList", "Customer");
        }

        [Authorize(Policy = ("CustomerPolicy"))]
        [HttpPost]
        public async Task<IActionResult> IncreaseCart(CartModel cart)
        {
            var token = User.Claims.FirstOrDefault(x => x.Type == "accesToken")?.Value;
            if (token != null)
            {
                var jwtId = User.Claims.FirstOrDefault(claim => claim.Type == "nameid")?.Value;
                int jwtID = int.Parse(jwtId);

                cart.CustomerID = jwtID;
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var data = JsonConvert.SerializeObject(cart);
                var content = new StringContent(data, Encoding.UTF8, "application/json");
                var response = await client.PostAsync($"http://localhost:5101/api/Cart/IncreaseCartProduct", content);
                if (response.IsSuccessStatusCode)
                {
                    var contents = await response.Content.ReadAsStringAsync();
                    cart = JsonConvert.DeserializeObject<CartModel>(contents);
                }
            }
            return RedirectToAction("CartList", "Customer");
        }






        [Authorize(Policy ="CustomerPolicy")]
        public async Task<IActionResult> CartList(int ID)
        {
            List<CustomerCartListModel> cartList = new List<CustomerCartListModel>();
            if (ModelState.IsValid)
            {
                
                var token = User.Claims.FirstOrDefault(x => x.Type == "accesToken")?.Value;
                var jwtId = User.Claims.FirstOrDefault(claim => claim.Type == "nameid")?.Value;
                int jwtID = int.Parse(jwtId);
                
                ID = jwtID;

                if (token != null)
                {

                    var client = _httpClientFactory.CreateClient();
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);


                 

                    var response = await client.GetAsync($"http://localhost:5101/api/Customer/CartList/{ID}");
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        cartList= JsonConvert.DeserializeObject<List<CustomerCartListModel>>(content);
                        return View(cartList);
                       
                    }
                }
                
            }
            return RedirectToAction("CartList", "Customer");
        }

        public async Task<IActionResult> PurchasedProductList(int ID) 
        {
        
            if (ModelState.IsValid)
            {

                var token = User.Claims.FirstOrDefault(x => x.Type == "accesToken")?.Value;
                var jwtId = User.Claims.FirstOrDefault(claim => claim.Type == "nameid")?.Value;
                int jwtID = int.Parse(jwtId);

                ID = jwtID;

                if (token != null)
                {

                    var client = _httpClientFactory.CreateClient();
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);




                    var response = await client.GetAsync($"http://localhost:5101/api/Customer/PurchasedProductList/{ID}");
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var cartList = JsonConvert.DeserializeObject<List<List<PurchaseProduct>>>(content);
                        return View(cartList);

                    }
                }

            }
            return RedirectToAction("PurchasedCartList", "Customer");
        }



        [Authorize(Policy ="CustomerPolicy")]
        public async Task<IActionResult> AddProductCart(CartModel cart)
        {
            if (ModelState.IsValid)
            {
                var token = User.Claims.FirstOrDefault(x => x.Type == "accesToken")?.Value;
                var jwtId = User.Claims.FirstOrDefault(claim => claim.Type == "nameid")?.Value;

               
                int jwtID = int.Parse(jwtId);
                cart.CustomerID = jwtID;

                if (token != null)
                {

                    var client = _httpClientFactory.CreateClient();
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);


                    var data = JsonConvert.SerializeObject(cart);



                    var content = new StringContent(data, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync($"http://localhost:5101/api/Customer/AddProductCart", content);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index", "Home");
                    }   
                }
            }
            else
            {
                return RedirectToAction("Login", "Customer");
            }
            return View(cart);
        }


    }


}
