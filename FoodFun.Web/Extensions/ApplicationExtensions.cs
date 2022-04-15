namespace FoodFun.Web.Extensions
{
    using Infrastructure.Data;
    using Infrastructure.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;

    using static Constants.GlobalConstants.Roles;

    public static class ApplicationExtensions
    {
        public static void MigrateDatabaseAndSeed(this WebApplication app)
        {
            var scope = app.Services.CreateScope();

            var dbContext = scope.ServiceProvider.GetService<FoodFunDbContext>();
            dbContext.Database.Migrate();

            if (!dbContext.Roles.Any())
            {
                SeedRoles(scope.ServiceProvider.GetService<RoleManager<IdentityRole>>())
                    .GetAwaiter()
                    .GetResult();
            }

            if (!dbContext.Users.Any())
            {
                var configuration = scope.ServiceProvider.GetService<IConfiguration>();

                SeedAdminUser(
                    scope.ServiceProvider.GetService<UserManager<User>>(),
                    configuration["AdminUser:Username"],
                    configuration["AdminUser:Password"],
                    configuration["AdminUser:Email"])
                    .GetAwaiter()
                    .GetResult();
            }

            if (!dbContext.ProductsCategories.Any())
            {
                SeedCategoriesForProduct(dbContext);
            }

            if(!dbContext.Products.Any())
            {
                SeedProducts(dbContext);
            }

            if (!dbContext.DishesCategories.Any())
            {
                SeedCategoriesForDish(dbContext);
            }

            if (!dbContext.Dishes.Any())
            {
                SeedDishes(dbContext);
            }

            if (!dbContext.TableSizes.Any())
            {
                SeedTableSizes(dbContext);
            }

            if (!dbContext.TablePositions.Any())
            {
                SeedTablePositions(dbContext);
            }

            if (!dbContext.Tables.Any())
            {
                SeedTable(dbContext);
            }
        }

        private async static Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole(Administrator));
            await roleManager.CreateAsync(new IdentityRole(OrderManager));
            await roleManager.CreateAsync(new IdentityRole(RestaurantManager));
            await roleManager.CreateAsync(new IdentityRole(SupermarketManager));
        }

        private async static Task SeedAdminUser(
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

        private static void SeedCategoriesForProduct(FoodFunDbContext dbContext)
        {
            var productCategoriesSet = File.ReadAllText("DataSets/ProductCategory/productCategories.json");

            var categories = JsonConvert.DeserializeObject<List<ProductCategory>>(productCategoriesSet);

            dbContext.ProductsCategories
                .AddRange(categories);

            dbContext.SaveChanges();
        }

        private static void SeedProducts(FoodFunDbContext dbContext)
        {
            var productsSet = File.ReadAllText("DataSets/Product/products.json");

            var products = JsonConvert.DeserializeObject<List<Product>>(productsSet);

            dbContext.Products
                .AddRange(products);

            dbContext.SaveChanges();
        }

        private static void SeedDishes(FoodFunDbContext dbContext)
        {
            var dishesSet = File.ReadAllText("DataSets/Dish/dishes.json");

            var dishes = JsonConvert.DeserializeObject<List<Dish>>(dishesSet);

            dbContext.Dishes
                .AddRange(dishes);

            dbContext.SaveChanges();
        }

        private static void SeedCategoriesForDish(FoodFunDbContext dbContext)
        {
            var dishCategoriesSet = File.ReadAllText("DataSets/DishCategory/dishCategories.json");

            List<DishCategory> categories = JsonConvert.DeserializeObject<List<DishCategory>>(dishCategoriesSet);

            dbContext.DishesCategories
                .AddRange(categories);

            dbContext.SaveChanges();
        }

        private static void SeedTableSizes(FoodFunDbContext dbContext)
        {
            var tableSizesSet = File.ReadAllText("DataSets/TableSize/tableSizes.json");

            var tableSizes = JsonConvert.DeserializeObject<List<TableSize>>(tableSizesSet);

            dbContext.TableSizes
                .AddRange(tableSizes);

            dbContext.SaveChanges();
        }

        private static void SeedTablePositions(FoodFunDbContext dbContext)
        {
            var tablePositionsSet = File.ReadAllText("DataSets/TablePosition/tablePositions.json");

            var tablePositions = JsonConvert.DeserializeObject<List<TablePosition>>(tablePositionsSet);

            dbContext.TablePositions
                .AddRange(tablePositions);

            dbContext.SaveChanges();
        }

        private static void SeedTable(FoodFunDbContext dbContext)
        {
            var tablesSet = File.ReadAllText("DataSets/Table/tables.json");

            var tables = JsonConvert.DeserializeObject<List<Table>>(tablesSet);

            dbContext.Tables
                .AddRange(tables);

            dbContext.SaveChanges();
        }
    }
}
