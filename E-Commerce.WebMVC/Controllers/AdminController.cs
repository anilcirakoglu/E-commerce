using E_Commerce.WebMVC.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using X.PagedList;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace E_Commerce.WebMVC.Controllers
{

    public class AdminController : Controller
    {
        readonly private IHttpClientFactory _httpClientFactory;

        public AdminController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public IActionResult Login()
        {
            return View(new LoginModel());
        }
       
        public IActionResult SignIn()
        {
            return View(new AdminModel());
        }
        

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var client = this._httpClientFactory.CreateClient();
                var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

                var response = await client.PostAsync("http://localhost:5101/api/Admin/Login", content);

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

        public async Task<IActionResult> Edit()
        {
            var admin = new AdminModel();
            if (ModelState.IsValid)
            {
                var token = User.Claims.FirstOrDefault(x => x.Type == "accesToken")?.Value;

                var jwtId = User.Claims.FirstOrDefault(claim => claim.Type == "nameid")?.Value;
                int jwtID = int.Parse(jwtId);
                int ID = jwtID;
                admin.ID = jwtID;

                if (token != null)
                {

                    var client = _httpClientFactory.CreateClient();
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                    var response = await client.GetAsync($"http://localhost:5101/api/Admin/{ID}");

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();

                        var adminInfo = JsonConvert.DeserializeObject<AdminModel>(content);

                        return View(adminInfo);
                    }
                    ModelState.AddModelError("", "wrong Model");
                }
            }
            return View(admin);
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpPost]
        public async Task<IActionResult> Edit(AdminModel adminModel)
        {
            if (ModelState.IsValid)
            {
                var token = User.Claims.FirstOrDefault(x => x.Type == "accesToken")?.Value;

                var jwtId = User.Claims.FirstOrDefault(claim => claim.Type == "nameid")?.Value;
                int jwtID = int.Parse(jwtId);
                adminModel.ID = jwtID;//Düzenleyebilir miyim diye bak

                if (token != null)
                {

                    var client = _httpClientFactory.CreateClient();
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                    var data = JsonConvert.SerializeObject(adminModel);
                    var content = new StringContent(data, Encoding.UTF8, "application/json");

                    var response = await client.PutAsync($"http://localhost:5101/api/Admin/Update", content);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    ModelState.AddModelError("", "wrong Model");
                }
            }
            return View(adminModel);
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(AdminModel admin)
        {
            if (ModelState.IsValid)
            {
                var token = User.Claims.FirstOrDefault(x => x.Type == "accesToken")?.Value;


                if (token != null)
                {
                    var client = _httpClientFactory.CreateClient();
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                    var data = JsonConvert.SerializeObject(admin);
                    var content = new StringContent(data, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync("http://localhost:5101/api/Admin/Registration", content);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    ModelState.AddModelError("", "wrong Model");

                }

            }
            return View(admin);
        }
        [Authorize(Policy = "AdminPolicy")]
        [HttpGet]
        public async Task<IActionResult> ListSeller(int page=1)
        {
            List<SellerModel> sellers = new List<SellerModel>();
            var token = User.Claims.FirstOrDefault(x => x.Type == "accesToken")?.Value;
            if (token != null)
            {
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                var response = await client.GetAsync("http://localhost:5101/api/Seller");
                if (response.IsSuccessStatusCode)
                {

                    var content = await response.Content.ReadAsStringAsync();
                    sellers = JsonConvert.DeserializeObject<List<SellerModel>>(content);
                    var pageList = sellers.ToPagedList(page, 5);
                    return View(pageList);
                }
               
            }
            return View("ListSeller", sellers);
        }
        [Authorize(Policy = "AdminPolicy")]
        [HttpGet]
        public async Task<IActionResult> ListCustomer(int page=1)
        {
            List<CustomerModel> customers = new List<CustomerModel>();
            var token = User.Claims.FirstOrDefault(x => x.Type == "accesToken")?.Value;
            if (token != null)
            {
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                var response = await client.GetAsync("http://localhost:5101/api/Customer");
                if (response.IsSuccessStatusCode)
                {

                    var content = await response.Content.ReadAsStringAsync();
                    customers = JsonConvert.DeserializeObject<List<CustomerModel>>(content);
                    var pageList = customers.ToPagedList(page, 5);
                    return View(pageList);
                }
               
            }
            return View(customers);

        }
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> ApprovedSeller(int ID)
        {



            var token = User.Claims.FirstOrDefault(x => x.Type == "accesToken")?.Value;
            if (token != null)
            {
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var data = JsonConvert.SerializeObject(ID);
                var content = new StringContent(data, Encoding.UTF8, "application/json");
                var response = await client.PostAsync($"http://localhost:5101/api/Admin/ApprovedSeller", content);
                if (response.IsSuccessStatusCode)
                {

                    var contents = await response.Content.ReadAsStringAsync();
                    ID = JsonConvert.DeserializeObject<int>(contents);

                   
                }
               
            }

            return RedirectToAction("ListSeller");


        }
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> RejectSeller(int ID)
        {
            var token = User.Claims.FirstOrDefault(x => x.Type == "accesToken")?.Value;
            if (token != null)
            {
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var data = JsonConvert.SerializeObject(ID);
                var content = new StringContent(data, Encoding.UTF8, "application/json");
                var response = await client.PostAsync($"http://localhost:5101/api/Admin/RejectSeller", content);
                if (response.IsSuccessStatusCode)
                {

                    var contents = await response.Content.ReadAsStringAsync();
                    ID = JsonConvert.DeserializeObject<int>(contents);
                }
               
            }

            return RedirectToAction("ListSeller");


        }




        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> ApproveProduct(int ID)
        {
            


            var token = User.Claims.FirstOrDefault(x => x.Type == "accesToken")?.Value;
            if (token != null)
            {
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var data = JsonConvert.SerializeObject(ID);
                var content = new StringContent(data, Encoding.UTF8, "application/json");
                var response = await client.PostAsync($"http://localhost:5101/api/Admin/ApproveProduct", content);
                if (response.IsSuccessStatusCode)
                {

                    var contents = await response.Content.ReadAsStringAsync();
                    ID = JsonConvert.DeserializeObject<int>(contents);
                }
                else
                {

                    // Örneğin, loglama, hata mesajı gösterme veya başka bir işlem
                }
            }

            return RedirectToAction("ListProduct","Product");


        }
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> RejectProduct(int ID)
        {



            var token = User.Claims.FirstOrDefault(x => x.Type == "accesToken")?.Value;
            if (token != null)
            {
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var data = JsonConvert.SerializeObject(ID);
                var content = new StringContent(data, Encoding.UTF8, "application/json");
                var response = await client.PostAsync($"http://localhost:5101/api/Admin/RejectProduct", content);
                if (response.IsSuccessStatusCode)
                {

                    var contents = await response.Content.ReadAsStringAsync();
                    ID = JsonConvert.DeserializeObject<int>(contents);
                }
                else
                {

                    // Örneğin, loglama, hata mesajı gösterme veya başka bir işlem
                }
            }

            return RedirectToAction("ListProduct", "Product");


        }

       
     
    }
}
