using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient();



builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddCookie
     (JwtBearerDefaults.AuthenticationScheme, opt =>
     {
         opt.LoginPath = "/Customer/Login";
         opt.AccessDeniedPath = "/CategoryProduct/AccessDenied";//bak yerini deðiþtir
         opt.Cookie.SameSite = SameSiteMode.Strict;//cookie ilgili domain çalýsýr
         opt.Cookie.HttpOnly = true;
         opt.Cookie.Name = "Cookie";

     });
builder.Services.AddAuthorization(options =>
{
    
    options.AddPolicy("AdminPolicy", policy =>
    {
        policy.AddAuthenticationSchemes(
                  JwtBearerDefaults.AuthenticationScheme);
        //policy.RequireAuthenticatedUser();
        policy.RequireClaim("role", "Admin");
       
    });
    options.AddPolicy("SellerPolicy", policy =>
    {
        policy.AddAuthenticationSchemes(
                  JwtBearerDefaults.AuthenticationScheme);
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("role", "Seller");
    });
    options.AddPolicy("CustomerPolicy", policy =>
    {
        policy.AddAuthenticationSchemes(
                  JwtBearerDefaults.AuthenticationScheme);
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("role", "Customer");
    });

    
}
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");

    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
