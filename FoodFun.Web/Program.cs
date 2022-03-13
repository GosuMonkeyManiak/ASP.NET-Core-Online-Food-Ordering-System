using FoodFun.Core.Contracts;
using FoodFun.Core.Services;
using FoodFun.Infrastructure.Common.Contracts;
using FoodFun.Infrastructure.Data;
using FoodFun.Infrastructure.Data.Repositories;
using FoodFun.Infrastructure.Models;
using FoodFun.Web.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder
    .Configuration
    .GetConnectionString("DefaultConnection");

builder
    .Services
    .AddDbContext<FoodFunDbContext>(options => options
        .UseSqlServer(connectionString));

builder
    .Services
    .AddDatabaseDeveloperPageExceptionFilter();

builder
    .Services
    .AddIdentity<User, IdentityRole>(options => { options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromHours(1); })
    .AddEntityFrameworkStores<FoodFunDbContext>();

builder
    .Services
    .ConfigureApplicationCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromDays(10);
        options.LoginPath = "/Identity/Account/Login";
        options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    });

builder
    .Services
    .AddControllersWithViews(options => { options.Filters.Add<AutoValidateAntiforgeryTokenAttribute>(); });

builder
    .Services
    .AddTransient<IProductService, ProductService>()
    .AddTransient<IProductRepository, ProductRepository>()
    .AddTransient<IProductCategoryService, ProductCategoryService>()
    .AddTransient<IDishService, DishService>()
    .AddTransient<IDishCategoryService, DishCategoryService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.MigrateDatabaseAndSeed();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    "DefaultArea",
    "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    "default",
    "{controller=Home}/{action=Index}/{id?}");

app.Run();