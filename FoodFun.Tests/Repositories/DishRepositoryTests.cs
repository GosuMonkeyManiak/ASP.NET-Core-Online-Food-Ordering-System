namespace FoodFun.Tests.Repositories
{
    using FoodFun.Infrastructure.Common.Contracts;
    using FoodFun.Infrastructure.Data;
    using FoodFun.Infrastructure.Data.Repositories;
    using Infrastructure.Models;
    using Microsoft.EntityFrameworkCore;
    using NUnit.Framework;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    [TestFixture]
    public class DishRepositoryTests
    {
        private IDishRepository dishRepository;

        private readonly DbContextOptions<FoodFunDbContext> dbContextOptions;
        private FoodFunDbContext dbContext;

        public DishRepositoryTests()
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

            this.dishRepository = new DishRepository(this.dbContext);
        }

        private static void AssertTwoCollections(
            List<Dish> expectedProducts,
            List<Dish> actualProducts)
        {
            for (int i = 0; i < expectedProducts.Count; i++)
            {
                AssertTwoProducts(expectedProducts[i], actualProducts[i]);
            }
        }

        private static void AssertTwoProducts(
            Dish expectedProduct,
            Dish actualProduct)
        {
            Assert.AreEqual(expectedProduct.Id, actualProduct.Id);
            Assert.AreEqual(expectedProduct.Name, actualProduct.Name);
            Assert.AreEqual(expectedProduct.ImageUrl, actualProduct.ImageUrl);
            Assert.AreEqual(expectedProduct.CategoryId, actualProduct.CategoryId);
            Assert.AreEqual(expectedProduct.Price, actualProduct.Price);
            Assert.AreEqual(expectedProduct.Description, actualProduct.Description);
            Assert.AreEqual(expectedProduct.Quantity, actualProduct.Quantity);
        }

        private static IQueryable<Dish> AddFilters(
            IQueryable<Dish> query,
            string searchTerm,
            int categoryFilterId,
            bool onlyAvailable)
        {
            if (searchTerm != null)
            {
                query = query
                    .Where(x => x.Name.Contains(searchTerm) || x.Description.Contains(searchTerm));
            }

            if (categoryFilterId != 0)
            {
                query = query
                    .Where(x => x.CategoryId == categoryFilterId);
            }

            if (onlyAvailable)
            {
                query = query
                    .Where(x => !x.Category.IsDisable && x.Quantity > 0);
            }

            return query;
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
        public async Task When_CallDishesWithCategoriesPrivateProperty_ShouldReturnNotAttachedProductsWithCategories()
        {
            await SeedTestDishes();

            var dishesWithCategoriesPropertyValue = this.dishRepository
                .GetType()
                .GetProperty("DishesWithCategories", BindingFlags.NonPublic | BindingFlags.Instance)
                .GetValue(this.dishRepository);

            Assert.IsInstanceOf<IQueryable<Dish>>(dishesWithCategoriesPropertyValue);

            var actualDishesWithCategories = await (dishesWithCategoriesPropertyValue as IQueryable<Dish>)
                .ToListAsync();

            Assert.AreEqual(this.dbContext.Dishes.Count(), actualDishesWithCategories.Count);

            var expectedProductsWithCategories = await this.dbContext.Dishes
                .Include(c => c.Category)
                .AsNoTracking()
                .ToListAsync();

            AssertTwoCollections(expectedProductsWithCategories, actualDishesWithCategories);
        }

        [Test]
        public async Task When_CallGetDishesWithCategories_ShouldReturn_AllCategories()
        {
            await SeedTestDishes();

            var actualDishesWithCategories = (await this.dishRepository.GetDishesWithCategories())
                .ToList();

            var expectedDishesWithCategories = await this.dbContext
                .Dishes
                .Include(c => c.Category)
                .AsNoTracking()
                .ToListAsync();

            Assert.IsNotNull(actualDishesWithCategories);
            Assert.AreEqual(expectedDishesWithCategories.Count, actualDishesWithCategories.Count);

            AssertTwoCollections(expectedDishesWithCategories, actualDishesWithCategories);
        }

        [Test]
        [TestCase("c3182fae-c620-4b79-be5c-0f05e104f9ea")]
        [TestCase("82496e1f-3120-4016-860e-e98558678477")]
        public async Task When_CallDishWithCategoryById_ShouldReturnDish(string id)
        {
            await SeedTestDishes();

            var actualProduct = await this.dishRepository.GetDishWithCategoryById(id);

            var expected = await this.dbContext
                .Dishes
                .AsNoTracking()
                .FirstAsync(x => x.Id == id);

            Assert.IsNotNull(actualProduct);

            AssertTwoProducts(expected, actualProduct);
        }

        [Test]
        [TestCase("ba", 0, false)]
        [TestCase(null, 2, true)]
        [TestCase("c", 1, false)]
        public async Task When_CallAddFilters_ShouldAddCorrect(
            string searchTerm,
            int categoryFilterId,
            bool onlyAvailable)
        {
            await SeedTestDishes();

            var addFilterMethod = this.dishRepository
                .GetType()
                .GetMethod("AddFilters", BindingFlags.NonPublic | BindingFlags.Instance);

            var actualDishes = ((IQueryable<Dish>)addFilterMethod.Invoke(this.dishRepository, new object[] { this.dbContext
                .Dishes
                .Include(c => c.Category)
                .AsNoTracking(), searchTerm, categoryFilterId, onlyAvailable }))
                .ToList();

            var expectedDishesQuery = this.dbContext
                .Dishes
                .Include(c => c.Category)
                .AsQueryable();

            expectedDishesQuery = AddFilters(
                expectedDishesQuery,
                searchTerm,
                categoryFilterId,
                onlyAvailable);

            var expectedDishes = await expectedDishesQuery.ToListAsync();

            Assert.NotNull(actualDishes);
            Assert.AreEqual(expectedDishes.Count, actualDishes.Count);

            AssertTwoCollections(expectedDishes, actualDishes);
        }

        [Test]
        [TestCase("ba", 1, true)]
        [TestCase(null, 0, true)]
        [TestCase("c", 3, true)]
        public async Task When_CallGetCountOfItemsByFilters_ShouldReturnCorrectCount(
            string searchTerm,
            int categoryFilterId,
            bool onlyAvailable)
        {
            await SeedTestDishes();

            var actualDishesCount = await this.dishRepository
                .GetCountOfItemsByFilters(searchTerm, categoryFilterId, onlyAvailable);

            var expectedDishesQuery = this.dbContext
                .Dishes
                .Include(c => c.Category)
                .AsQueryable();

            expectedDishesQuery = AddFilters(expectedDishesQuery, searchTerm, categoryFilterId, onlyAvailable);

            var expectedDishesCount = (await expectedDishesQuery.ToListAsync()).Count;

            Assert.AreEqual(expectedDishesCount, actualDishesCount);
        }

        [Test]
        [TestCase("ba", 0, 0, 1, 1, false)]
        [TestCase(null, 2, 1, 1, 1, true)]
        [TestCase("c", 1, 1, 1, 2, false)]
        public async Task When_CallGetAllProductsWithCategories_ShouldReturnCorrectFilteredProducts(
            string searchTerm,
            int categoryFilterId,
            byte orderNumber,
            int pageNumber,
            int pageSize,
            bool onlyAvailable)
        {
            await SeedTestDishes();

            var actualFilteredDishes = (await this.dishRepository
                .GetAllDishesWithCategories(
                    searchTerm,
                    categoryFilterId,
                    orderNumber,
                    pageNumber,
                    pageSize,
                    onlyAvailable))
                    .ToList();

            var filteredQuery = this.dbContext
                .Dishes
                .Include(c => c.Category)
                .AsNoTracking();

            filteredQuery = AddFilters(filteredQuery, searchTerm, categoryFilterId, onlyAvailable);

            if (orderNumber == 1)
            {
                filteredQuery = filteredQuery
                    .OrderByDescending(x => x.Price);
            }
            else
            {
                filteredQuery = filteredQuery
                    .OrderBy(x => x.Price);
            }

            var exprectedFilteredDishes = await filteredQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            Assert.AreEqual(exprectedFilteredDishes.Count, actualFilteredDishes.Count);

            AssertTwoCollections(exprectedFilteredDishes, actualFilteredDishes);
        }
    }
}
