namespace FoodFun.Tests.ProductCategory
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Threading.Tasks;
    using AutoMapper;
    using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
    using Core.AutoMapper;
    using Core.Contracts;
    using Core.Services;
    using Infrastructure.Common.Contracts;
    using Infrastructure.Models;
    using Microsoft.Extensions.DependencyInjection;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class ProductCategoryServiceTests
    {
        private IProductCategoryService productCategoryService;
        private Mock<IProductCategoryRepository> productCategoryRepositoryMock;
        private readonly IMapper mapper;

        private IList<ProductCategory> productCategories;
        private IList<Product> products;

        public ProductCategoryServiceTests()
        {
            var services = new ServiceCollection();
            services.AddAutoMapper(typeof(AutoMapperServiceProfile));
            var serviceProvider = services.BuildServiceProvider();

            this.mapper = serviceProvider.GetService<IMapper>();
        }

        [SetUp]
        public void SetUp()
        {
            this.productCategoryRepositoryMock = new Mock<IProductCategoryRepository>();

            this.productCategoryService = new ProductCategoryService(
                this.productCategoryRepositoryMock.Object, this.mapper);

            this.productCategories = new List<ProductCategory>();

            MockRepositoryMethods();
        }

        private void MockRepositoryMethods()
        {
            this.productCategoryRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<ProductCategory>()))
                .Callback<ProductCategory>(x => this.productCategories.Add(x));

            this.productCategoryRepositoryMock
                .Setup(x => x.AllAsNoTracking())
                .ReturnsAsync(this.productCategories);

            this.productCategoryRepositoryMock
                .Setup(x => x.GetAllNotDisabled())
                .ReturnsAsync(this.productCategories.Where(x => !x.IsDisable));

            this.productCategoryRepositoryMock
                .Setup(x => x.FindOrDefaultAsync(It.IsAny<Expression<Func<ProductCategory, bool>>>()))
                .Returns<Expression<Func<ProductCategory, bool>>>(async x =>
                    await Task.FromResult(this.productCategories.FirstOrDefault(x.Compile())));

            this.productCategoryRepositoryMock
                .Setup(x => x.GetAllCategoriesWithProducts())
                .ReturnsAsync(this.productCategories);

            this.productCategoryRepositoryMock
                .Setup(x => x.GetCategoryWithProductsById(It.IsAny<int>()))
                .Returns<int>(async x => await Task.FromResult(this.productCategories.First(a => a.Id == x)));

            this.productCategoryRepositoryMock
                .Setup(x => x.Update(It.IsAny<ProductCategory>()))
                .Callback<ProductCategory>(x =>
                {
                    var category = this.productCategories.First(a => a.Id == x.Id);
                    category.Title = x.Title;
                    category.IsDisable = x.IsDisable;
                });
        }

        private void SeedTestCategories()
        {
            var bananas = new Product()
            {
                Id = "4da54227-ccf1-4fd5-9fb5-21ae4356da33",
                Name = "Bananas",
                ImageUrl = "bananas.jpg",
                Price = 12.20M,
                Description = "Test bananas",
                Quantity = 120
            };
            var watermelon = new Product()
            {
                Id = "058e7d03-7082-4d92-9fa3-b0458afd484f",
                Name = "WaterMelon",
                ImageUrl = "waterMelon.jpg",
                Price = 10.20M,
                Description = "Test waterMelon",
                Quantity = 40
            };
            var tomatoes = new Product()
            {
                Id = "999cae77-6db9-4437-bb4f-440bcfcc8772",
                Name = "Tomatoes",
                ImageUrl = "tomatos.jpg",
                Price = 9.20M,
                Description = "Test tomatos",
                Quantity = 12
            };
            var chicken = new Product()
            {
                Id = "22164c80-b0de-4633-ada5-a74ac0674843",
                Name = "Chicken",
                ImageUrl = "chicken.jpg",
                Price = 12,
                Description = "Test chicken",
                Quantity = 0
            };

            this.productCategories.Add(new ProductCategory()
            {
                Id = 1,
                Title = "Meat",
                IsDisable = true,
                Products = new List<Product>() { chicken }
            });
            this.productCategories.Add(new ProductCategory()
            {
                Id = 2,
                Title = "Fruits",
                Products = new List<Product>() { bananas, watermelon }
            });
            this.productCategories.Add(new ProductCategory()
            {
                Id = 3,
                Title = "Vegetables",
                IsDisable = true,
                Products = new List<Product>() { tomatoes }
            });
            this.productCategories.Add(new ProductCategory()
            {
                Id = 4,
                Title = "Eggs",
            });
        }

        [Test]
        public void When_CreateCategoryService_FieldShouldBeSetted()
        {
            var productCategoryType = this.productCategoryService
                .GetType();

            var repoFieldValue = productCategoryType
                .GetField("productCategoryRepository", BindingFlags.NonPublic | BindingFlags.Instance)
                .GetValue(this.productCategoryService);

            var mapperFieldValue = productCategoryType
                .GetField("mapper", BindingFlags.NonPublic | BindingFlags.Instance)
                .GetValue(this.productCategoryService);

            Assert.AreEqual(this.productCategoryRepositoryMock.Object, repoFieldValue);
            Assert.AreEqual(this.mapper, mapperFieldValue);
        }

        [Test]
        [TestCase("meat")]
        [TestCase("vegetables")]
        [TestCase("fruits")]
        [TestCase("eggs")]
        [TestCase("sea food")]
        public async Task When_CallAddOnCategory_ShouldBeAdded(string title)
        {
            await this.productCategoryService.Add(title);

            var actualProductCategory = this.productCategories.First(x => x.Title == title);
            var expectedProductCategory = new ProductCategory() { Title = title, IsDisable = false };

            Assert.AreEqual(1, this.productCategories.Count);
            Assert.AreEqual(expectedProductCategory.Title, actualProductCategory.Title);
            Assert.AreEqual(expectedProductCategory.IsDisable, actualProductCategory.IsDisable);
        }

        [Test]
        public async Task When_CallAll_ShouldReturnAllCategories()
        {
            SeedTestCategories();

            var actualResult = (await this.productCategoryService
                .All()).ToList();
            var expectedResult = this.productCategories;

            Assert.AreEqual(expectedResult.Count, actualResult.Count);

            for (int i = 0; i < expectedResult.Count; i++)
            {
                Assert.AreEqual(expectedResult[i].Id, actualResult[i].Id);
                Assert.AreEqual(expectedResult[i].Title, actualResult[i].Title);
            }
        }

        [Test]
        public async Task When_CallAllNotDisabled_ShouldReturnOnlyActiveCategories()
        {
            SeedTestCategories();

            var actualResult = (await this.productCategoryService
                    .AllNotDisabled()).ToList();
            var expectedResult = this.productCategories
                .Where(x => !x.IsDisable)
                .ToList();

            Assert.AreEqual(expectedResult.Count, actualResult.Count);

            for (int i = 0; i < expectedResult.Count; i++)
            {
                Assert.AreEqual(expectedResult[i].Id, actualResult[i].Id);
                Assert.AreEqual(expectedResult[i].Title, actualResult[i].Title);
            }
        }

        [Test]
        [TestCase(2)]
        [TestCase(4)]
        public async Task When_CallIsCategoryActiveWithActiveCategoryId_ShouldReturnTrue(int id)
        {
            SeedTestCategories();

            var result = await this.productCategoryService
                .IsCategoryActive(id);

            Assert.IsTrue(result);
        }

        [Test]
        [TestCase(1)]
        [TestCase(3)]
        public async Task When_CallIsCategoryActiveWithDisableCategoryId_ShouldReturnFalse(int id)
        {
            SeedTestCategories();

            var result = await this.productCategoryService
                .IsCategoryActive(id);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task When_CallAllWithProductCount_ShouldReturnAllCategoryWithProductCount()
        {
            SeedTestCategories();

            var actualResult = (await this.productCategoryService
                .AllWithProductsCount())
                .ToList();

            var expectedResult = this.productCategories;

            Assert.AreEqual(expectedResult.Count, actualResult.Count);

            for (int i = 0; i < expectedResult.Count; i++)
            {
                Assert.AreEqual(expectedResult[i].Id, actualResult[i].Id);
                Assert.AreEqual(expectedResult[i].Title, actualResult[i].Title);
                Assert.AreEqual(expectedResult[i].IsDisable, actualResult[i].IsDisable);
                Assert.AreEqual(expectedResult[i].Products.Count, actualResult[i].ProductsCount);
            }
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public async Task When_CallIsCategoryExistWithValidCategoryId_ShouldReturnTrue(int id)
        {
            SeedTestCategories();

            var actualResult = await this.productCategoryService
                .IsCategoryExist(id);

            Assert.IsTrue(actualResult);
        }

        [Test]
        [TestCase(120)]
        [TestCase(23)]
        [TestCase(30)]
        [TestCase(49)]
        public async Task When_CallIsCategoryExistWithInValidCategoryId_ShouldReturnFalse(int id)
        {
            SeedTestCategories();

            var actualResult = await this.productCategoryService
                .IsCategoryExist(id);

            Assert.IsFalse(actualResult);
        }

        [Test]
        [TestCase("Meat")]
        [TestCase("Fruits")]
        [TestCase("Vegetables")]
        [TestCase("Eggs")]
        public async Task When_CallIsCategoryExistWithValidTitle_ShouldReturnTrue(string title)
        {
            SeedTestCategories();

            var actualResult = await this.productCategoryService
                .IsCategoryExist(title);

            Assert.IsTrue(actualResult);
        }

        [Test]
        [TestCase("apples")]
        [TestCase("sea food")]
        [TestCase("coffee")]
        [TestCase("drinks")]
        public async Task When_CallIsCategoryExistWithInValidTitle_ShouldReturnTrue(string title)
        {
            SeedTestCategories();

            var actualResult = await this.productCategoryService
                .IsCategoryExist(title);

            Assert.IsFalse(actualResult);
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public async Task When_CallGetByIdOrDefaultWithValidId_ShouldReturnCategory(int id)
        {
            SeedTestCategories();

            var actualResult = await this.productCategoryService
                .GetByIdOrDefault(id);

            var expectedResult = this.productCategories
                .First(x => x.Id == id);

            Assert.NotNull(actualResult);

            Assert.AreEqual(expectedResult.Id, actualResult.Id);
            Assert.AreEqual(expectedResult.Title, actualResult.Title);
        }

        [Test]
        [TestCase(120)]
        [TestCase(20)]
        [TestCase(39)]
        [TestCase(420)]
        public async Task When_CallGetByIdOrDefaultWithInValidId_ShouldReturnNull(int id)
        {
            SeedTestCategories();

            var actualResult = await this.productCategoryService
                .GetByIdOrDefault(id);

           Assert.IsNull(actualResult);
        }

        [Test]
        [TestCase(1, "Chicken")]
        [TestCase(2, "Bananas")]
        [TestCase(2, "WaterMelon")]
        [TestCase(3, "Tomatoes")]
        public async Task When_CallIsProductExistInCategoryWithValidParameter_ShouldReturnTrue(
            int categoryId,
            string productName)
        {
            SeedTestCategories();

            var actualResult = await this.productCategoryService
                .IsProductExistInCategory(categoryId, productName);

            Assert.IsTrue(actualResult);
        }

        [Test]
        [TestCase(1, "Bananas")]
        [TestCase(2, "Chicken")]
        [TestCase(2, "Chicken")]
        [TestCase(3, "Chicken")]
        public async Task When_CallIsProductExistInCategoryWithInValidProductName_ShouldReturnFalse(
            int categoryId,
            string productName)
        {
            SeedTestCategories();

            var actualResult = await this.productCategoryService
                .IsProductExistInCategory(categoryId, productName);

            Assert.IsFalse(actualResult);
        }

        [Test]
        [TestCase(2)]
        [TestCase(4)]
        public async Task When_CallDisable_Category_ShouldChangeIsDisableToTrue(int id)
        {
            SeedTestCategories();

            await this.productCategoryService
                .Disable(id);

            var actualResult = this.productCategories.First(x => x.Id == id).IsDisable;
            var expectedResult = true;

            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        [TestCase(1)]
        [TestCase(3)]
        public async Task When_CallEnable_Category_ShouldChangeIsDisableToFalse(int id)
        {
            SeedTestCategories();

            await this.productCategoryService
                .Enable(id);

            var actualResult = this.productCategories.First(x => x.Id == id).IsDisable;
            var expectedResult = false;

            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        [TestCase(1, "Test1")]
        [TestCase(2, "Test2")]
        [TestCase(3, "Test3")]
        [TestCase(4, "Test3")]
        public async Task When_CallUpdate_CategoryTitle_ShouldBeChange(
            int categoryId, string title)
        {
            SeedTestCategories();

            await this.productCategoryService
                .Update(categoryId, title);

            var actualResult = this.productCategories.First(x => x.Id == categoryId);

            Assert.AreEqual(categoryId, actualResult.Id);
            Assert.AreEqual(title, actualResult.Title);
        }
    }
}
