namespace FoodFun.Tests.Repositories
{
    using FoodFun.Infrastructure.Common.Contracts;
    using FoodFun.Infrastructure.Data;
    using FoodFun.Infrastructure.Data.Repositories;
    using FoodFun.Infrastructure.Models;
    using Microsoft.EntityFrameworkCore;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    [TestFixture]
    public class ProductRepositoryTests
    {
        private IProductRepository productRepository;

        private readonly DbContextOptions<FoodFunDbContext> dbContextOptions;
        private FoodFunDbContext dbContext;

        public ProductRepositoryTests()
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
            this.dbContext.Database.EnsureDeleted();

            this.productRepository = new ProductRepository(this.dbContext);
        }

        private static void AssertTwoCollections(
            List<Product> expectedProducts, 
            List<Product> actualProducts)
        {
            for (int i = 0; i < expectedProducts.Count; i++)
            {
                AssertTwoProducts(expectedProducts[i], actualProducts[i]);
            }
        }

        private static void AssertTwoProducts(
            Product expectedProduct, 
            Product actualProduct)
        {
            Assert.AreEqual(expectedProduct.Id, actualProduct.Id);
            Assert.AreEqual(expectedProduct.Name, actualProduct.Name);
            Assert.AreEqual(expectedProduct.ImageUrl, actualProduct.ImageUrl);
            Assert.AreEqual(expectedProduct.CategoryId, actualProduct.CategoryId);
            Assert.AreEqual(expectedProduct.Price, actualProduct.Price);
            Assert.AreEqual(expectedProduct.Description, actualProduct.Description);
            Assert.AreEqual(expectedProduct.Quantity, actualProduct.Quantity);
        }

        private static IQueryable<Product> AddFilters(
            IQueryable<Product> query,
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
        public async Task When_CallProductsWithCategoriesPrivateProperty_ShouldReturnNotAttachedProductsWithCategories()
        {
            await SeedTestProductsAndCategories();

            var productsWithCategoriesPropertyValue = this.productRepository
                .GetType()
                .GetProperty("ProductsWithCategories", BindingFlags.NonPublic | BindingFlags.Instance)
                .GetValue(this.productRepository);

            Assert.IsInstanceOf<IQueryable<Product>>(productsWithCategoriesPropertyValue);
            
            var actualProductWithCategories = await (productsWithCategoriesPropertyValue as IQueryable<Product>)
                .ToListAsync();

            Assert.AreEqual(this.dbContext.Products.Count(), actualProductWithCategories.Count);

            var expectedProductsWithCategories = await this.dbContext.Products
                .Include(c => c.Category)
                .AsNoTracking()
                .ToListAsync();

            AssertTwoCollections(expectedProductsWithCategories, actualProductWithCategories);
        }

        [Test]
        [TestCase("4da54227-ccf1-4fd5-9fb5-21ae4356da33", "058e7d03-7082-4d92-9fa3-b0458afd484f")]
        [TestCase("999cae77-6db9-4437-bb4f-440bcfcc8772", "22164c80-b0de-4633-ada5-a74ac0674843")]
        public async Task When_CallAll_WithIds_ShouldReturnProductWhichMatchSomeId(params string[] ids)
        {
            await SeedTestProductsAndCategories();

            var actualProductsWithCategories = (await this.productRepository.All(ids))
                .ToList();

            var expectedProductsWithCategories = await this.dbContext.Products
                .AsNoTracking()
                .Where(x => ids.Contains(x.Id))
                .ToListAsync();

            Assert.IsNotNull(actualProductsWithCategories);
            Assert.AreEqual(expectedProductsWithCategories.Count, actualProductsWithCategories.Count);

            AssertTwoCollections(expectedProductsWithCategories, actualProductsWithCategories);
        }

        [Test]
        [TestCase("4da54227-ccf1-4fd5-9fb5-21ae4356da33")]
        [TestCase("058e7d03-7082-4d92-9fa3-b0458afd484f")]
        public async Task When_CallGetProductWithCategoryById_ShouldReturnProduct(string id)
        {
            await SeedTestProductsAndCategories();

            var actualProduct = await this.productRepository.GetProductWithCategoryById(id);

            var expected = await this.dbContext
                .Products
                .AsNoTracking()
                .FirstAsync(x => x.Id == id);

            Assert.IsNotNull(actualProduct);

            AssertTwoProducts(expected, actualProduct);
        }

        [Test]
        [TestCase("ba", 0, true)]
        [TestCase(null, 2, true)]
        [TestCase("c", 1, false)]
        public async Task When_CallAddFilters_ShouldAddCorrect(
            string searchTerm,
            int categoryFilterId,
            bool onlyAvailable)
        {
            await SeedTestProductsAndCategories();

            var addFilterMethod = this.productRepository
                .GetType()
                .GetMethod("AddFilters", BindingFlags.NonPublic | BindingFlags.Instance);

            var actualProducts = ((IQueryable<Product>) addFilterMethod.Invoke(this.productRepository, new object[] { this.dbContext
                .Products
                .Include(c => c.Category)
                .AsNoTracking(), searchTerm, categoryFilterId, onlyAvailable }))
                .ToList();

            var expectedProductsQuery = this.dbContext
                .Products
                .Include(c => c.Category)
                .AsQueryable();

            expectedProductsQuery = AddFilters(
                expectedProductsQuery,
                searchTerm,
                categoryFilterId,
                onlyAvailable);

            var expectedProducts = await expectedProductsQuery.ToListAsync();

            Assert.NotNull(actualProducts);
            Assert.AreEqual(expectedProducts.Count, actualProducts.Count);

            AssertTwoCollections(expectedProducts, actualProducts);
        }

        [Test]
        [TestCase("ba", 0, true)]
        [TestCase(null, 2, true)]
        [TestCase("c", 1, false)]
        public async Task When_CallGetCountOfItemsByFilters_ShouldReturnCorrectCount(
            string searchTerm,
            int categoryFilterId,
            bool onlyAvailable)
        {
            await SeedTestProductsAndCategories();

            var actualProductsCount = await this.productRepository
                .GetCountOfItemsByFilters(searchTerm, categoryFilterId, onlyAvailable);

            var expectedProductsQuery = this.dbContext
                .Products
                .Include(c => c.Category)
                .AsQueryable();

            expectedProductsQuery = AddFilters(expectedProductsQuery, searchTerm, categoryFilterId, onlyAvailable);

            var expectedProductsCount = (await expectedProductsQuery.ToListAsync()).Count;

            Assert.AreEqual(expectedProductsCount, actualProductsCount);
        }

        [Test]
        [TestCase("ba", 0, 0, 1, 1, true)]
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
            await SeedTestProductsAndCategories();

            var actualFilteredProducts = (await this.productRepository
                .GetAllProductsWithCategories(
                    searchTerm,
                    categoryFilterId, 
                    orderNumber, 
                    pageNumber, 
                    pageSize, 
                    onlyAvailable))
                    .ToList();

            var filteredQuery = this.dbContext
                .Products
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

            var exprectedFilteredProducts = await filteredQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            Assert.AreEqual(exprectedFilteredProducts.Count, actualFilteredProducts.Count);

            AssertTwoCollections(exprectedFilteredProducts, actualFilteredProducts);
        }
    }
}
