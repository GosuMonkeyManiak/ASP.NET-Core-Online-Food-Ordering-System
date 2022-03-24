using FoodFun.Core.AutoMapper;
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
    .AddStackExchangeRedisCache(options =>
    {
        options.Configuration = builder.Configuration.GetConnectionString("Redis");
        options.InstanceName = "Redis-Store";
    });

builder
    .Services
    .AddSession(options =>
    {
        options.Cookie.Name = "sid";
        options.IdleTimeout = TimeSpan.FromHours(10);
        options.Cookie.HttpOnly = true;
        options.Cookie.IsEssential = true;
    });

builder
    .Services
    .AddDatabaseDeveloperPageExceptionFilter();

builder
    .Services
    .AddDefaultIdentity<User>(options => 
    { 
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromHours(1);
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<FoodFunDbContext>();

builder
    .Services
    .ConfigureApplicationCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromDays(10);
    });

builder
    .Services
    .AddControllersWithViews(options =>
    {
        options.Filters.Add<AutoValidateAntiforgeryTokenAttribute>();
    });

builder
    .Services
    .AddAutoMapper(typeof(AutoMapperServiceProfile), typeof(Program));

builder
    .Services
    .AddScoped<IProductRepository, ProductRepository>()
    .AddScoped<IProductCategoryRepository, ProductCategoryRepository>()
    .AddScoped<IDishRepository, DishRepository>()
    .AddScoped<IDishCategoryRepository, DishCategoryRepository>();

builder
    .Services
    .AddTransient<IProductService, ProductService>()
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

app.UseSession();

app.MapControllerRoute(
    "DefaultArea",
    "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    "default",
    "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();