using E_Commerce.WebMVC.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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

        [HttpPost]//Düzenleme gerekli admin onayı için if şartını unutma
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

                ModelState.AddModelError(string.Empty, "Email or Password Wrong");
                return View(model);

            }
            return View(model);
        }
        public IActionResult Logout()// kontrol et html de 
        {

            Response.Cookies.Delete("Cookie");

            return RedirectToAction("Index", "Home");
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
    }
}
