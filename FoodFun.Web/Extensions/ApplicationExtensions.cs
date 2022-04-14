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

            //if (!db.Roles.Any())
            //{
            //    SeedRoles(scope.ServiceProvider.GetService<RoleManager<IdentityRole>>())
            //        .GetAwaiter()
            //        .GetResult();
            //}

            //if (!db.Users.Any())
            //{
            //    var configuration = scope.ServiceProvider.GetService<IConfiguration>();

            //    SeedUsers(
            //        scope.ServiceProvider.GetService<UserManager<User>>(),
            //        configuration["AdminUser:Username"],
            //        configuration["AdminUser:Password"],
            //        configuration["AdminUser:Email"])
            //        .GetAwaiter()
            //        .GetResult();
            //}

            if (!dbContext.ProductsCategories.Any())
            {
                SeedCategoriesForProduct(dbContext);
            }

            //if (!db.DishesCategories.Any())
            //{
            //    SeedCategoriesForDish(db)
            //        .GetAwaiter()
            //        .GetResult();
            //}

            //if (!db.TableSizes.Any())
            //{
            //    SeedTableSizes(db).GetAwaiter().GetResult();
            //}

            //if (!db.TablePositions.Any())
            //{
            //    SeedTablePositions(db).GetAwaiter().GetResult();
            //}

            //if (!db.Tables.Any())
            //{
            //    SeedTable(db).GetAwaiter().GetResult();
            //}
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

        private static void SeedCategoriesForProduct(FoodFunDbContext dbContext)
        {
            var productCategoriesSet = File.ReadAllText("DataSets/ProductCategory/productCategories.json");

            List<ProductCategory> categories = JsonConvert.DeserializeObject<List<ProductCategory>>(productCategoriesSet);

            dbContext.ProductsCategories
                .AddRange(categories);

            dbContext.SaveChanges();
        }

        private async static Task SeedCategoriesForDish(FoodFunDbContext dbContext)
        {
            List<DishCategory> categories = new()
            {
                new() { Title = "Salad" },
                new() { Title = "Soup" },
                new() { Title = "Drinks" },
                new() { Title = "BBQ" },
                new() { Title = "Pizza" },
            };

            await dbContext.DishesCategories
                .AddRangeAsync(categories);

            await dbContext.SaveChangesAsync();
        }

        private async static Task SeedTableSizes(FoodFunDbContext dbContext)
        {
            var tableSizes = new List<TableSize>()
            {
                new TableSize() { Seats = 4 },
                new TableSize() { Seats = 5 },
                new TableSize() { Seats = 6 },
                new TableSize() { Seats = 7 },
                new TableSize() { Seats = 8 },
                new TableSize() { Seats = 9 },
                new TableSize() { Seats = 10 },
            };

            await dbContext.TableSizes
                .AddRangeAsync(tableSizes);

            await dbContext.SaveChangesAsync();
        }

        private async static Task SeedTablePositions(FoodFunDbContext dbContext)
        {
            var tablePositions = new List<TablePosition>()
            {
                new TablePosition() { Position = "Out side" },
                new TablePosition() { Position = "In side" }
            };

            await dbContext.TablePositions
                .AddRangeAsync(tablePositions);

            await dbContext.SaveChangesAsync();
        }

        private async static Task SeedTable(FoodFunDbContext dbContext)
        {
            var tables = new List<Table>()
            {
                new Table() { TableSizeId = 1, TablePositionId = 1},
                new Table() { TableSizeId = 2, TablePositionId = 1},
                new Table() { TableSizeId = 3, TablePositionId = 1},
                new Table() { TableSizeId = 4, TablePositionId = 1},
                new Table() { TableSizeId = 5, TablePositionId = 1},
                new Table() { TableSizeId = 6, TablePositionId = 1},
                new Table() { TableSizeId = 7, TablePositionId = 1},
                new Table() { TableSizeId = 1, TablePositionId = 1},
                new Table() { TableSizeId = 2, TablePositionId = 1},
                new Table() { TableSizeId = 3, TablePositionId = 1},

                new Table() { TableSizeId = 1, TablePositionId = 2},
                new Table() { TableSizeId = 2, TablePositionId = 2},
                new Table() { TableSizeId = 3, TablePositionId = 2},
                new Table() { TableSizeId = 4, TablePositionId = 2},
                new Table() { TableSizeId = 5, TablePositionId = 2},
                new Table() { TableSizeId = 6, TablePositionId = 2},
                new Table() { TableSizeId = 7, TablePositionId = 2},
                new Table() { TableSizeId = 1, TablePositionId = 2},
                new Table() { TableSizeId = 2, TablePositionId = 2},
                new Table() { TableSizeId = 3, TablePositionId = 2},
            };

            await dbContext.Tables
                .AddRangeAsync(tables);

            await dbContext.SaveChangesAsync();
        }
    }
}
