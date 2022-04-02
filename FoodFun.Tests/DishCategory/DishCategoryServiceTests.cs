namespace FoodFun.Tests.DishCategory
{
    using AutoMapper;
    using System;
    using FoodFun.Core.AutoMapper;
    using FoodFun.Core.Contracts;
    using FoodFun.Core.Services;
    using FoodFun.Infrastructure.Common.Contracts;
    using Microsoft.Extensions.DependencyInjection;
    using Moq;
    using NUnit.Framework;
    using System.Collections.Generic;
    using FoodFun.Infrastructure.Models;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Linq;
    using System.Linq.Expressions;

    [TestFixture]
    public class DishCategoryServiceTests
    {
        private IDishCategoryService dishCategoryService;

        private Mock<IDishCategoryRepository> dishCategoryRepo;
        private IMapper mapper;

        private IList<DishCategory> dishCategories;

        public DishCategoryServiceTests()
        {
            var serviceCollection = new ServiceCollection()
                .AddAutoMapper(typeof(AutoMapperServiceProfile));
            var serviceProvider = serviceCollection.BuildServiceProvider();

            this.mapper = serviceProvider.GetService<IMapper>();
            this.dishCategoryRepo = new Mock<IDishCategoryRepository>();

            this.dishCategoryService = new DishCategoryService(
                this.dishCategoryRepo.Object,
                this.mapper);
        }

        [SetUp]
        public void SetUp()
        {
            this.dishCategories = new List<DishCategory>();

            MockRepoMethods();
        }

        private void MockRepoMethods()
        {
            this.dishCategoryRepo
                .Setup(x => x.AddAsync(It.IsAny<DishCategory>()))
                .Callback<DishCategory>(dishcategory => this.dishCategories.Add(dishcategory));

            this.dishCategoryRepo
                .Setup(x => x.AllAsNoTracking())
                .ReturnsAsync(this.dishCategories);

            this.dishCategoryRepo
                .Setup(x => x.GetAllWithDishes())
                .ReturnsAsync(this.dishCategories);

            this.dishCategoryRepo
                .Setup(x => x.FindAsync(It.IsAny<Expression<Func<DishCategory, bool>>>()))
                .Returns<Expression<Func<DishCategory, bool>>>(async x =>
                    await Task.FromResult(this.dishCategories.First(x.Compile())));

            this.dishCategoryRepo
                .Setup(x => x.Update(It.IsAny<DishCategory>()))
                .Callback<DishCategory>(x =>
                {
                    var category = this.dishCategories.First(a => a.Id == x.Id);
                    category.Title = x.Title;
                    category.IsDisable = x.IsDisable;
                });

            this.dishCategoryRepo
                .Setup(x => x.FindOrDefaultAsync(It.IsAny<Expression<Func<DishCategory, bool>>>()))
                .Returns<Expression<Func<DishCategory, bool>>>(async x => await Task.FromResult(this.dishCategories.FirstOrDefault(x.Compile())));
        }

        private void SeedTestCategories()
        {
            var pizzaChicago = new Dish()
            {
                Id = "c9270c5e-7fe4-4ade-95c4-a977fb3380f6",
                Name = "Chicago",
                ImageUrl = "Chicago.jpg",
                CategoryId = 3,
                Description = "great pizza",
                Price = 15.20M,
                Quantity = 12
            };
            var greekSalad = new Dish()
            {
                Id = "be641dcf-fa61-4d04-9c98-61a9d558788a",
                Name = "Greek Salad",
                ImageUrl = "greek.jpg",
                CategoryId = 2,
                Description = "great salad",
                Price = 12.20M,
                Quantity = 2
            };
            var steak = new Dish()
            {
                Id = "bb1bf854-a140-424d-91bf-e89f4805446f",
                Name = "Steak",
                ImageUrl = "steak.jpg",
                CategoryId = 1,
                Description = "great steak",
                Price = 22.20M,
                Quantity = 10
            };
            var chickenSoup = new Dish()
            {
                Id = "aff19008-bafb-4fef-82c6-c2fc76cbd816",
                Name = "Chicken soup",
                ImageUrl = "chicke.jpg",
                CategoryId = 4,
                Description = "great soup",
                Price = 9.20M,
                Quantity = 5
            };

            this.dishCategories.Add(new DishCategory()
            {
                Id = 1, 
                Title = "BBQ",
                Dishes = new List<Dish> { steak }
            });
            this.dishCategories.Add(new DishCategory() 
            { 
                Id = 2, 
                Title = "Salads",
                IsDisable = true,
                Dishes = new List<Dish> { greekSalad }
            });
            this.dishCategories.Add(new DishCategory() 
            { 
                Id = 3, 
                Title = "Pizza",
                Dishes = new List<Dish> { pizzaChicago }
            });
            this.dishCategories.Add(new DishCategory() 
            { 
                Id = 4, 
                Title = "Soups",
                IsDisable = true,
                Dishes = new List<Dish> { chickenSoup }
            });
        }

        [Test]
        public void When_CreateCategoryService_BackingField_ShouldBeSetted()
        {
            var categoryServiceType = this.dishCategoryService
                .GetType();

            var dishCategoryRepositoryFieldValue = categoryServiceType
                .GetField("dishCategoryRepository", BindingFlags.NonPublic | BindingFlags.Instance)
                .GetValue(this.dishCategoryService);

            var mapperFieldValue = categoryServiceType
                .GetField("mapper", BindingFlags.NonPublic | BindingFlags.Instance)
                .GetValue(this.dishCategoryService);

            Assert.AreEqual(this.dishCategoryRepo.Object, dishCategoryRepositoryFieldValue);
            Assert.AreEqual(this.mapper, mapperFieldValue);
        }

        [Test]
        [TestCase("BBQs")]
        [TestCase("Salads")]
        [TestCase("Pizza")]
        [TestCase("Soups")]
        public async Task When_CallAdd_ShouldAddDishCategoryToBackingStore(string title)
        {
            await this.dishCategoryService
                .Add(title);

            var actualResult = this.dishCategories.First();

            Assert.AreEqual(1, this.dishCategories.Count);
            Assert.AreEqual(title, actualResult.Title);
        }

        [Test]
        public async Task When_CallAll_ShouldReturnCollectionOfDishCategories()
        {
            SeedTestCategories();

            var actualResult = (await this.dishCategoryService.All()).ToList();

            Assert.AreEqual(this.dishCategories.Count, actualResult.Count);

            for (int i = 0; i < this.dishCategories.Count; i++)
            {
                Assert.AreEqual(this.dishCategories[i].Id, actualResult[i].Id);
                Assert.AreEqual(this.dishCategories[i].Title, actualResult[i].Title);
            }
        }

        [Test]
        public async Task When_CallAllWithDishesCount_ShouldReturnCollectionWithDishCategories()
        {
            var allCategoriesWithDishesCount =(await this.dishCategoryService
                .AllWithDishesCount())
                .ToList();

            Assert.AreEqual(this.dishCategories.Count, allCategoriesWithDishesCount.Count);

            for (int i = 0; i < this.dishCategories.Count; i++)
            {
                Assert.AreEqual(this.dishCategories[i].Id, allCategoriesWithDishesCount[i].Id);
                Assert.AreEqual(this.dishCategories[i].Title, allCategoriesWithDishesCount[i].Title);
                Assert.AreEqual(this.dishCategories[i].Dishes.Count, allCategoriesWithDishesCount[i].Count);
            }
        }

        [Test]
        [TestCase(1)]
        [TestCase(3)]
        public async Task When_CallDisable_ShouldChangeIsDisableToTrue(int id)
        {
            SeedTestCategories();

            await this.dishCategoryService
                .Disable(id);

            var actualResult = this.dishCategories
                .First(x => x.Id == id).IsDisable;

            Assert.IsTrue(actualResult);
        }

        [Test]
        [TestCase(2)]
        [TestCase(4)]
        public async Task When_CallEnable_ShouldChangeIsDisableToFalse(int id)
        {
            SeedTestCategories();

            await this.dishCategoryService
                .Enable(id);

            var actualResult = this.dishCategories
                .First(x => x.Id == id).IsDisable;

            Assert.IsFalse(actualResult);
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public async Task When_CallIsCategoryExistByValidId_ShouldReturnTrue(int id)
        {
            SeedTestCategories();

            var actualResult = await this.dishCategoryService
                .IsCategoryExist(id);

            Assert.IsTrue(actualResult);
        }

        [Test]
        [TestCase(12)]
        [TestCase(20)]
        [TestCase(39)]
        [TestCase(42)]
        public async Task When_CallIsCategoryExistByInValidId_ShouldReturnFalse(int id)
        {
            SeedTestCategories();

            var actualResult = await this.dishCategoryService
                .IsCategoryExist(id);

            Assert.IsFalse(actualResult);
        }

        [Test]
        [TestCase(1)]
        [TestCase(3)]
        public async Task When_CallIsCategoryActive_WithActiveCategoryId_ShouldReturnTrue(int id)
        {
            SeedTestCategories();

            var actualResult = await this.dishCategoryService
                .IsCategoryActive(id);

            Assert.IsTrue(actualResult);
        }

        [Test]
        [TestCase(2)]
        [TestCase(4)]
        public async Task When_CallIsCategoryActive_WithInActiveCategoryId_ShouldReturnFalse(int id)
        {
            SeedTestCategories();

            var actualResult = await this.dishCategoryService
                .IsCategoryActive(id);

            Assert.IsFalse(actualResult);
        }

        [Test]
        [TestCase("BBQ")]
        [TestCase("Salads")]
        [TestCase("Pizza")]
        [TestCase("Soups")]
        public async Task When_CallIsCategoryExist_WithValidTitle_ShouldReturnTrue(string title)
        {
            SeedTestCategories();

            var actualResult = await this.dishCategoryService
                .IsCategoryExist(title);

            Assert.IsTrue(actualResult);
        }

        [Test]
        [TestCase("Drinks")]
        [TestCase("salad")]
        public async Task When_CallIsCategoryExist_WithInValidTitle_ShouldReturnFalse(string title)
        {
            SeedTestCategories();

            var actualResult = await this.dishCategoryService
                .IsCategoryExist(title);

            Assert.IsFalse(actualResult);
        }

        [Test]
        [TestCase(1, "Steak")]
        [TestCase(2, "Greek Salad")]
        [TestCase(3, "Chicago")]
        [TestCase(4, "Chicken soup")]
        public async Task When_CallIsItemExistInCategory_WithValidParameters_ShouldReturnTrue(
            int categoryId, string itemName)
        {
            SeedTestCategories();

            var actualResult = await this.dishCategoryService
                .IsItemExistInCategory(categoryId, itemName);

            Assert.IsTrue(actualResult);
        }

        [Test]
        [TestCase(1, "Greek Salad")]
        [TestCase(2, "Steak")]
        [TestCase(3, "Chicken soup")]
        [TestCase(4, "Chicago")]
        public async Task When_CallIsItemExistInCategory_WithInValidParameters_ShouldReturnFalse(
            int categoryId, string itemName)
        {
            SeedTestCategories();

            var actualResult = await this.dishCategoryService
                .IsItemExistInCategory(categoryId, itemName);

            Assert.IsFalse(actualResult);
        }

        [Test]
        [TestCase("bb1bf854-a140-424d-91bf-e89f4805446f")]
        [TestCase("c9270c5e-7fe4-4ade-95c4-a977fb3380f6")]
        public async Task When_CallIsItemInAnyActiveCategory_WithValidItemId_ShouldReturnTrue(string itemId)
        {
            SeedTestCategories();

            var actualResult = await this.dishCategoryService
                .IsItemInAnyActiveCategory(itemId);

            Assert.IsTrue(actualResult);
        }

        [Test]
        [TestCase("be641dcf-fa61-4d04-9c98-61a9d558788a")]
        [TestCase("aff19008-bafb-4fef-82c6-c2fc76cbd816")]
        public async Task When_CallIsItemInAnyActiveCategory_WithInValidItemId_ShouldReturnFalse(string itemId)
        {
            SeedTestCategories();

            var actualResult = await this.dishCategoryService
                .IsItemInAnyActiveCategory(itemId);

            Assert.IsFalse(actualResult);
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public async Task When_CallGetByIdOrDefault_WithValidId_ShouldReturnDishCategory(int id)
        {
            SeedTestCategories();

            var actualResult = await this.dishCategoryService
                .GetByIdOrDefault(id);

            Assert.IsNotNull(actualResult);
        }

        [Test]
        [TestCase(1, "Drinks")]
        [TestCase(2, "BBQ")]
        [TestCase(3, "Soups")]
        [TestCase(4, "Pizza")]
        public async Task When_CallUpdate_TheCategory_ShouldBeUpdated(int id, string title)
        {
            SeedTestCategories();

            await this.dishCategoryService
                .Update(id, title);

            var actualResult = this.dishCategories.First(x => x.Id == id);

            Assert.AreEqual(title, actualResult.Title);
        }

        [Test]
        [TestCase(120)]
        [TestCase(240)]
        [TestCase(323)]
        [TestCase(410)]
        public async Task When_CallGetByIdOrDefault_WithInValidId_ShouldReturnNull(int id)
        {
            SeedTestCategories();

            var actualResult = await this.dishCategoryService
                .GetByIdOrDefault(id);

            Assert.IsNull(actualResult);
        }
    }
}
