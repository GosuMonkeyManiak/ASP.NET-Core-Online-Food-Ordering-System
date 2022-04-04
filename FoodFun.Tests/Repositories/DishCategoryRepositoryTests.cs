namespace FoodFun.Tests.Repositories
{
    using NUnit.Framework;
    using Infrastructure.Common.Contracts;
    using Microsoft.EntityFrameworkCore;
    using Infrastructure.Data;
    using Infrastructure.Data.Repositories;
    using Infrastructure.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Linq;

    [TestFixture]
    public class DishCategoryRepositoryTests
    {
        private IDishCategoryRepository dishCategoryRepository;

        private readonly DbContextOptions<FoodFunDbContext> dbContextOptions;
        private FoodFunDbContext dbContext;

        public DishCategoryRepositoryTests()
        {
            this.dbContextOptions = new DbContextOptionsBuilder<FoodFunDbContext>()
                .UseInMemoryDatabase("FoodFunTest")
                .Options;
        }

        [SetUp]
        public void SetUp()
        {
            this.dbContext = new FoodFunDbContext(this.dbContextOptions);

            this.dbContext.Database.EnsureDeleted();
            this.dbContext.Database.EnsureCreated();

            this.dishCategoryRepository = new DishCategoryRepository(this.dbContext);
        }

        private static void AssertTwoCollections(
            List<DishCategory> expectedProducts,
            List<DishCategory> actualProducts)
        {
            for (int i = 0; i < expectedProducts.Count; i++)
            {
                AssertTwoCategories(expectedProducts[i], actualProducts[i]);
            }
        }

        private static void AssertTwoCategories(
            DishCategory expectedProduct,
            DishCategory actualProduct)
        {
            Assert.AreEqual(expectedProduct.Id, actualProduct.Id);
            Assert.AreEqual(expectedProduct.Title, actualProduct.Title);
            Assert.AreEqual(expectedProduct.IsDisable, actualProduct.IsDisable);
            Assert.AreEqual(expectedProduct.Dishes.Count, actualProduct.Dishes.Count);
        }

        private async Task SeedTestDishes()
        {
            var soupsCategory = new DishCategory() { Id = 1, Title = "Soups" };
            var pizzaCategory = new DishCategory() { Id = 2, Title = "Pizza", IsDisable = true };
            var bbqCategory = new DishCategory() { Id = 3, Title = "BBQ" };
            var salads = new DishCategory() { Id = 4, Title = "Salads", IsDisable = true };

            var dishCategories = new List<DishCategory>();

            dishCategories.Add(soupsCategory);
            dishCategories.Add(pizzaCategory);
            dishCategories.Add(bbqCategory);
            dishCategories.Add(salads);

            await this.dbContext.DishesCategories.AddRangeAsync(dishCategories);

            await this.dbContext.SaveChangesAsync();

            var dishes = new List<Dish>();

            dishes.Add(new()
            {
                Id = "c3182fae-c620-4b79-be5c-0f05e104f9ea",
                Name = "Chicken soup",
                ImageUrl = "chickensoup.jpg",
                CategoryId = 1,
                Description = "chicken soup test",
                Price = 12.20M,
                Quantity = 20
            });
            dishes.Add(new()
            {
                Id = "82496e1f-3120-4016-860e-e98558678477",
                Name = "Sweet soup",
                ImageUrl = "sweet soup",
                CategoryId = 1,
                Description = "sweet soup",
                Price = 15.40M,
                Quantity = 1
            });
            dishes.Add(new()
            {
                Id = "81ba5e61-6ed8-454b-8419-67ebd4f16e74",
                Name = "Pizza",
                ImageUrl = "pizza.jpg",
                CategoryId = 2,
                Description = "pizza test",
                Price = 15.20M,
                Quantity = 10
            });
            dishes.Add(new()
            {
                Id = "5bc5fdbc-4bba-46fa-aa18-1f307a1d48ae",
                Name = "Steak",
                ImageUrl = "steak.jpg",
                CategoryId = 3,
                Description = "steak test",
                Price = 16.20M,
                Quantity = 3
            });
            dishes.Add(new()
            {
                Id = "38db3486-ea3b-44cb-8d27-8a68fd84ae30",
                Name = "Greek Salad",
                ImageUrl = "salad.jpg",
                CategoryId = 4,
                Description = "salad test",
                Price = 9.20M,
                Quantity = 14
            });

            await this.dbContext.Dishes.AddRangeAsync(dishes);

            await this.dbContext.SaveChangesAsync();
        }

        [Test]
        public async Task When_CallGetAllWithDishes_ShouldReturn_AllCategoriesWithDishes()
        {
            await SeedTestDishes();

            var actualCategories = (await this.dishCategoryRepository
                .GetAllWithDishes())
                .ToList();

            var expectedCategories = await this.dbContext
                .DishesCategories
                .Include(d => d.Dishes)
                .AsNoTracking()
                .ToListAsync();

            AssertTwoCollections(expectedCategories, actualCategories);
        }
    }
}
