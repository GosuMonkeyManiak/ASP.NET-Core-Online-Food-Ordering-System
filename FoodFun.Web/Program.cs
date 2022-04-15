using FoodFun.Web.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder
    .Services
    .AddDbContext(builder.Configuration)
    .AddDefaultIdentity()
    .AddRedisCache(builder.Configuration)
    .AddConfiguredSession()
    .AddAutoMapper()
    .AddRepositories()
    .AddServices()
    .AddDatabaseDeveloperPageExceptionFilter()
    .AddConfiguredCookies()
    .AddConfiguredControllersWithViews();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app
    .UseHttpsRedirection()
    .UseStaticFiles()
    .UseRouting()
    .UseAuthentication()
    .UseAuthorization()
    .UseSession();

app.MapControllerRoute(
    "DefaultArea",
    "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    "default",
    "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.MigrateDatabaseAndSeed();

app.Run();