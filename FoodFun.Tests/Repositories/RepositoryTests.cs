namespace FoodFun.Tests.Repositories
{
    using FoodFun.Infrastructure.Data.Repositories;
    using NUnit.Framework;
    using FoodFun.Infrastructure.Models;
    using Microsoft.EntityFrameworkCore;
    using FoodFun.Infrastructure.Data;
    using System.Reflection;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Linq;
    using FoodFun.Core.Extensions;

    [TestFixture]
    public class RepositoryTests
    {
        private EfRepository<Product> efRepo;

        private readonly DbContextOptions<FoodFunDbContext> dbOptions;
        private FoodFunDbContext dbContext;

        public RepositoryTests()
        {
            this.dbOptions = new DbContextOptionsBuilder<FoodFunDbContext>()
                .UseInMemoryDatabase("FoodFunTest")
                .Options;
        }

        [SetUp]
        public async Task SetUp()
        {
            this.dbContext = new FoodFunDbContext(this.dbOptions);

            this.dbContext.Database.EnsureDeleted();
            this.dbContext.Database.EnsureCreated();

            efRepo = new ProductRepository(this.dbContext);
        }

        private async Task SeedTestProducts()
        {
            var products = new List<Product>();

            products.Add(new()
            {
                Id = "4da54227-ccf1-4fd5-9fb5-21ae4356da33",
                Name = "Bananas",
                ImageUrl = "bananas.jpg",
                Price = 12.20M,
                Description = "Test bananas",
                Quantity = 120
            });
            products.Add(new()
            {
                Id = "058e7d03-7082-4d92-9fa3-b0458afd484f",
                Name = "WaterMelon",
                ImageUrl = "waterMelon.jpg",
                Price = 10.20M,
                Description = "Test waterMelon",
                Quantity = 40
            });
            products.Add(new()
            {
                Id = "999cae77-6db9-4437-bb4f-440bcfcc8772",
                Name = "Tomatos",
                ImageUrl = "tomatos.jpg",
                Price = 9.20M,
                Description = "Test tomatos",
                Quantity = 12
            });
            products.Add(new()
            {
                Id = "22164c80-b0de-4633-ada5-a74ac0674843",
                Name = "Chicken",
                ImageUrl = "chicken.jpg",
                Price = 12,
                Description = "Test chicken",
                Quantity = 0
            });

            await this.dbContext
                .Products
                .AddRangeAsync(products);

            await this.dbContext.SaveChangesAsync();
        }

        [Test]
        public void When_CreateRepo_BackingFieldAndProperties_ShouldBeSetted()
        {
            var dbContextFieldValue = this.efRepo
                .GetType()
                .BaseType
                .GetField("dbContext", BindingFlags.NonPublic | BindingFlags.Instance)
                .GetValue(this.efRepo);

            var dbSetPropertyValue = this.efRepo
                .GetType()
                .BaseType
                .GetProperty("DbSet", BindingFlags.NonPublic | BindingFlags.Instance)
                .GetValue(this.efRepo);

            Assert.IsNotNull(dbContextFieldValue);
            Assert.IsNotNull(dbSetPropertyValue);

            Assert.AreEqual(this.dbContext, dbContextFieldValue);
            Assert.AreEqual(this.dbContext.Products, dbSetPropertyValue);
        }

        [Test]
        public async Task When_CallCount_ShouldReturnCorrectValue()
        {
            await SeedTestProducts();

            var actualResult = this.efRepo.Count;

            var expectedResult = await this.dbContext.Products.CountAsync();

            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        public async Task When_CallAll_Should_ReturnAll_AttachToDbContext()
        {
            await SeedTestProducts();

            var actualResult = await this.efRepo.All();

            var expectedCount = await this.dbContext.Products.CountAsync();

            Assert.AreEqual(expectedCount, actualResult.Count());

            foreach(var product in actualResult)
            {
                Assert.IsTrue(this.dbContext.IsAttach(product));
            }
        }

        [Test]
        public async Task When_CallAllAsNotTracking_Should_ReturnAll_NotAttachToDbContext()
        {
            await SeedTestProducts();

            var actualResult = await this.efRepo.AllAsNoTracking();

            var expectedCount = await this.dbContext.Products.CountAsync();

            Assert.AreEqual(expectedCount, actualResult.Count());

            foreach (var product in actualResult)
            {
                Assert.IsFalse(this.dbContext.IsAttach(product));
            }
        }

        [Test]
        [TestCase("Apples", "apples.jpg", 3.20, "best apples", 2ul)]
        [TestCase("Watermelon", "watermelon.jpg", 6.20, "best watermelon", 12ul)]
        [TestCase("Tomatos", "tomatos.jpg", 9.20, "best tomatos", 22ul)]
        public async Task When_CallAdd_ShouldAddToDb(
            string name,
            string imageUrl,
            decimal price,
            string description,
            ulong quantity)
        {
            var product = new Product()
            {
                Name = name,
                ImageUrl = imageUrl,
                Price = price,
                Description = description,
                Quantity = quantity
            };

            await this.efRepo.AddAsync(product);
            await this.efRepo.SaveChangesAsync();

            var actualProduct = this.dbContext.Products.First();

            Assert.AreEqual(1, this.dbContext.Products.Count());

            Assert.AreEqual(product.Id, actualProduct.Id);
            Assert.AreEqual(product.Name, actualProduct.Name);
            Assert.AreEqual(product.ImageUrl, actualProduct.ImageUrl);
            Assert.AreEqual(product.Price, actualProduct.Price);
            Assert.AreEqual(product.Description, actualProduct.Description);
            Assert.AreEqual(product.Quantity, actualProduct.Quantity);
        }

        [Test]
        [TestCase("4da54227-ccf1-4fd5-9fb5-21ae4356da33")]
        [TestCase("058e7d03-7082-4d92-9fa3-b0458afd484f")]
        public async Task When_CallRemove_ShouldRemove_EntityFromDb(string itemId)
        {
            await SeedTestProducts();

            var product = this.dbContext.Products.First(x => x.Id == itemId);

            this.efRepo.Remove(product);

            await this.efRepo.SaveChangesAsync();

            Assert.AreEqual(this.dbContext.Products.Count(), this.efRepo.Count);

            Assert.IsFalse(this.dbContext.Products.FirstOrDefault(x => x.Id == itemId) != null);
        }

        [Test]
        [TestCase("4da54227-ccf1-4fd5-9fb5-21ae4356da33", "Chicken wigns")]
        [TestCase("058e7d03-7082-4d92-9fa3-b0458afd484f", "Mayonesa")]
        public async Task When_CallUpdate_ShouldUpdate_EntityInDb(
            string itemId,
            string name)
        {
            await SeedTestProducts();

            var secondContext = new FoodFunDbContext(this.dbOptions);
            var secondRepo = new ProductRepository(secondContext);

            var product = secondContext
                .Products
                .AsNoTracking()
                .First(x => x.Id == itemId);

            product.Name = name;

            secondRepo.Update(product);

            await secondRepo.SaveChangesAsync();

            var updatedProduct = secondContext.Products
                .First(x => x.Id == itemId);

            Assert.IsNotNull(updatedProduct);
            Assert.AreEqual(name, updatedProduct.Name);
        }

        [Test]
        [TestCase("4da54227-ccf1-4fd5-9fb5-21ae4356da33")]
        [TestCase("058e7d03-7082-4d92-9fa3-b0458afd484f")]
        public async Task When_CallFindOrDefaultAsync_WithValidId_ShouldReturn_DetachedEntity(string itemId)
        {
            await SeedTestProducts();

            var product = await this.efRepo.FindOrDefaultAsync(x => x.Id == itemId);

            Assert.IsNotNull(product);
            Assert.IsFalse(this.dbContext.IsAttach(product));
        }

        [Test]
        [TestCase("4da54227-ccf1-4fd5-9fb5-21ae4356da3a")]
        [TestCase("058e7d03-7082-4d92-9fa3-b0458afd4841")]
        public async Task When_CallFindOrDefaultAsync_WithInValidId_ShouldReturn_Null(string itemId)
        {
            await SeedTestProducts();

            var product = await this.efRepo.FindOrDefaultAsync(x => x.Id == itemId);

            Assert.IsNull(product);
        }

        [Test]
        [TestCase("4da54227-ccf1-4fd5-9fb5-21ae4356da33")]
        [TestCase("058e7d03-7082-4d92-9fa3-b0458afd484f")]
        public async Task When_CallFindAsync_ShouldRetunr_DetachedEntity(string itemId)
        {
            await SeedTestProducts();

            var product = await this.efRepo.FindAsync(x => x.Id == itemId);

            Assert.IsNotNull(product);
            Assert.IsFalse(this.dbContext.IsAttach(product));
        }
    }
}
