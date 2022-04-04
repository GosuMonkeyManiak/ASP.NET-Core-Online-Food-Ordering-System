namespace FoodFun.Tests.Repositories
{
    using FoodFun.Infrastructure.Common.Contracts;
    using FoodFun.Infrastructure.Data;
    using FoodFun.Infrastructure.Data.Repositories;
    using FoodFun.Infrastructure.Models;
    using Microsoft.EntityFrameworkCore;
    using NUnit.Framework;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [TestFixture]
    public class ProductCategoryRepositoryTests
    {
        private IProductCategoryRepository productCategoryRepository;

        private readonly DbContextOptions<FoodFunDbContext> dbContextOptions;
        private FoodFunDbContext dbContext;

        public ProductCategoryRepositoryTests()
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

            this.productCategoryRepository = new ProductCategoryRepository(this.dbContext);
        }

        private static void AssertTwoCollections(
            List<ProductCategory> expectedProducts,
            List<ProductCategory> actualProducts)
        {
            for (int i = 0; i < expectedProducts.Count; i++)
            {
                AssertTwoCategories(expectedProducts[i], actualProducts[i]);
            }
        }

        private static void AssertTwoCategories(
            ProductCategory expectedProduct,
            ProductCategory actualProduct)
        {
            Assert.AreEqual(expectedProduct.Id, actualProduct.Id);
            Assert.AreEqual(expectedProduct.Title, actualProduct.Title);
            Assert.AreEqual(expectedProduct.IsDisable, actualProduct.IsDisable);
            Assert.AreEqual(expectedProduct.Products.Count, actualProduct.Products.Count);
        }

        private async Task SeedTestProductsAndCategories()
        {
            var fruitCategory = new ProductCategory() { Id = 1, Title = "Fruits" };
            var vegetablesCategory = new ProductCategory() { Id = 2, Title = "Vegetables", IsDisable = true };
            var meatCategory = new ProductCategory() { Id = 3, Title = "Meat" };

            var productCategories = new List<ProductCategory>();

            productCategories.Add(fruitCategory);
            productCategories.Add(vegetablesCategory);
            productCategories.Add(meatCategory);

            await this.dbContext.ProductsCategories.AddRangeAsync(productCategories);

            await this.dbContext.SaveChangesAsync();

            var products = new List<Product>();

            products.Add(new()
            {
                Id = "4da54227-ccf1-4fd5-9fb5-21ae4356da33",
                Name = "Bananas",
                ImageUrl = "bananas.jpg",
                CategoryId = fruitCategory.Id,
                Price = 12.20M,
                Description = "Test bananas",
                Quantity = 120
            });
            products.Add(new()
            {
                Id = "058e7d03-7082-4d92-9fa3-b0458afd484f",
                Name = "WaterMelon",
                ImageUrl = "waterMelon.jpg",
                CategoryId = fruitCategory.Id,
                Price = 10.20M,
                Description = "Test waterMelon",
                Quantity = 40
            });
            products.Add(new()
            {
                Id = "999cae77-6db9-4437-bb4f-440bcfcc8772",
                Name = "Tomatos",
                ImageUrl = "tomatos.jpg",
                CategoryId = vegetablesCategory.Id,
                Price = 9.20M,
                Description = "Test tomatos",
                Quantity = 12
            });
            products.Add(new()
            {
                Id = "22164c80-b0de-4633-ada5-a74ac0674843",
                Name = "Chicken",
                ImageUrl = "chicken.jpg",
                CategoryId = meatCategory.Id,
                Price = 12,
                Description = "Test chicken",
                Quantity = 0
            });

            await this.dbContext.Products.AddRangeAsync(products);

            await this.dbContext.SaveChangesAsync();
        }

        [Test]
        public async Task When_CallAllCategoriesWithProducts_ShouldReturnCorrectCollection()
        {
            await SeedTestProductsAndCategories();

            var actualCategories = (await this.productCategoryRepository
                .GetAllCategoriesWithProducts())
                .ToList();

            var expectedCategories = await this.dbContext
                .ProductsCategories
                .Include(p => p.Products)
                .AsNoTracking()
                .ToListAsync();

            AssertTwoCollections(expectedCategories, actualCategories);
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public async Task When_CallGetCategoryWithProductsById(int id)
        {
            await SeedTestProductsAndCategories();

            var actualCategories = await this.productCategoryRepository
                .GetCategoryWithProductsById(id);

            var expectedCategories = await this.dbContext
                .ProductsCategories
                .Include(p => p.Products)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            AssertTwoCategories(expectedCategories, actualCategories);
        }

        [Test]
        public async Task When_CallGetAllNotDisabled_ShouldReturnOnlyActiveCategories()
        {
            await SeedTestProductsAndCategories();

            var actualCategories = (await this.productCategoryRepository
                .GetAllNotDisabled())
                .ToList();

            var expectedCategories = await this.dbContext
                .ProductsCategories
                .AsNoTracking()
                .Where(x => !x.IsDisable)
                .ToListAsync();

            AssertTwoCollections(expectedCategories, actualCategories);
        }
    }
}
