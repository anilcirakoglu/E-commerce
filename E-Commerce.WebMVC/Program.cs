using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddCookie
     (JwtBearerDefaults.AuthenticationScheme,opt=>
     {
         opt.LoginPath = "/Customer/Login";
         opt.AccessDeniedPath="/Customer/AccessDenied";
         opt.AccessDeniedPath = "/CategoryProduct/AccessDenied";
         opt.Cookie.SameSite = SameSiteMode.Strict;//cookie ilgili domain çalýsýr
         opt.Cookie.HttpOnly = true;
         opt.Cookie.Name = "Cookie";
     });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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
