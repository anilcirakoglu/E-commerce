﻿using E_Commerce.WebMVC.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
        public IActionResult Edit()
        {
            return View(new CustomerModel());
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
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Email or Password Wrong");
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


                if (token != null)
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

        [HttpPost]
        public async Task<IActionResult> Edit(CustomerModel customer) 
        {
            if (ModelState.IsValid)
            {
                var token = User.Claims.FirstOrDefault(x => x.Type == "accesToken")?.Value;

                var jwtId = User.Claims.FirstOrDefault(claim => claim.Type == "nameid")?.Value;
                int jwtID = int.Parse(jwtId);
                customer.ID = jwtID;//Düzenleyebilir miyim diye bak

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
                    ModelState.AddModelError("", "wrong Model");

                }

            }
            return View(customer);
        }
    }


}