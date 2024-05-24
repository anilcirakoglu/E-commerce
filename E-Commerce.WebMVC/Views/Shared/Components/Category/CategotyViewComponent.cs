using E_Commerce.WebMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;

namespace E_Commerce.WebMVC.Views.Shared.Components.Category
{

    /// <summary>
    /// resource:https://learn.microsoft.com/en-us/aspnet/core/mvc/views/view-components?view=aspnetcore-8.0 
    /// </summary>
    [ViewComponent(Name = "Category")]
    public class CategotyViewComponent:ViewComponent
    {
        readonly private IHttpClientFactory _httpClientFactory;

        public CategotyViewComponent(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IViewComponentResult> InvokeAsync() 
        {
            List<CategoryProductModel> categoryProducts = new List<CategoryProductModel>();


            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("http://localhost:5101/api/CategoryProduct");
            if (response.IsSuccessStatusCode)
            {

                var content = await response.Content.ReadAsStringAsync();
                categoryProducts = JsonConvert.DeserializeObject<List<CategoryProductModel>>(content);

                return View(categoryProducts);

            }


            return View(categoryProducts);
           
        }
    }
}
