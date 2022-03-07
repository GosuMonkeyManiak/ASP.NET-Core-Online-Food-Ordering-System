namespace FoodFun.Web.Extensions
{
    using Infrastructure.Data;
    using Infrastructure.Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    using static Constants.GlobalConstants.Roles;

    public static class ApplicationExtensions
    {
        public static void MigrateDatabaseAndSeed(this WebApplication app)
        {
            var scope = app.Services.CreateScope();

            var db = scope.ServiceProvider.GetService<FoodFunDbContext>();

            db.Database.Migrate();

            if (!db.Roles.Any())
            {
                SeedRoles(scope.ServiceProvider.GetService<RoleManager<IdentityRole>>())
                    .GetAwaiter()
                    .GetResult();
            }

            if (!db.Users.Any())
            {
                var configuration = scope.ServiceProvider.GetService<IConfiguration>();

                SeedUsers(
                    scope.ServiceProvider.GetService<UserManager<User>>(),
                    configuration["AdminUser:Username"],
                    configuration["AdminUser:Password"],
                    configuration["AdminUser:Email"])
                    .GetAwaiter()
                    .GetResult();
            }
        }

        private async static Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole(Administrator));
            await roleManager.CreateAsync(new IdentityRole(OrderManager));
            await roleManager.CreateAsync(new IdentityRole(RestaurantManager));
        }

        private async static Task SeedUsers(
            UserManager<User> userManager,
            string userName,
            string password,
            string email)
        {
            var user = new User()
            {
                UserName = userName,
                Email = email,
            };

            await userManager.CreateAsync(user, password);

            await userManager.AddToRoleAsync(user, Administrator);
        }
    }
}
