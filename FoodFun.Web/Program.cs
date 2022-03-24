using FoodFun.Web.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder
    .Services
    .AddDbContext(builder.Configuration)
    .AddDefaultIdentity()
    .AddRedisCache(builder.Configuration)
    .AddSession()
    .AddAutoMapper()
    .AddRepositories()
    .AddServices()
    .AddDatabaseDeveloperPageExceptionFilter()
    .ConfigureCookies()
    .AddAndConfigureControllersWithViews();

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

app.Run();