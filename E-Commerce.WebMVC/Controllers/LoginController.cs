using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using E_Commerce.WebMVC.Models;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace E_Commerce.WebMVC.Controllers
{
    public class LoginController : Controller
    {
        readonly private IHttpClientFactory _httpClientFactory;

        public LoginController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public IActionResult Index() { 
        return View();
        }
        public async Task<IActionResult> LoginCustomer(LoginModel model)
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
                    return RedirectToAction("DisplayIndex", "Home");
                }

                ModelState.AddModelError(string.Empty, "Email or Password Wrong");
                return View(model);

            }
            return View(model);
        }
        [HttpPost]
        
        public async Task<IActionResult> LoginSeller(LoginModel model)
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
        public async Task<IActionResult> Logout()
        {

           

            await HttpContext.SignOutAsync(JwtBearerDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
