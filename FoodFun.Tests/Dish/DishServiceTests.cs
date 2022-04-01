namespace FoodFun.Tests.Dish
{
    using AutoMapper;
    using Core.AutoMapper;
    using Core.Contracts;
    using Core.Services;
    using Infrastructure.Common.Contracts;
    using Infrastructure.Models;
    using Microsoft.Extensions.DependencyInjection;
    using Moq;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Threading.Tasks;
    using Core.Models.DishCategory;

    [TestFixture]
    public class DishServiceTests
    {
        private IDishService dishService;
        private Mock<IDishRepository> dishRepoMock;
        private Mock<IDishCategoryService> dishCategoryServiceMock;
        private IMapper mapper;

        private readonly IServiceProvider serviceProvider;

        private IList<Dish> dishes;
        private IList<DishCategory> dishCategories;

        public DishServiceTests()
        {
            var services = new ServiceCollection()
                .AddAutoMapper(typeof(AutoMapperServiceProfile));
            this.serviceProvider = services.BuildServiceProvider();

            this.mapper = this.serviceProvider.GetService<IMapper>();

            this.dishRepoMock = new Mock<IDishRepository>();
            this.dishCategoryServiceMock = new Mock<IDishCategoryService>();

            this.dishService = new DishService(
                this.dishRepoMock.Object,
                this.dishCategoryServiceMock.Object,
                this.mapper);
        }

        [SetUp]
        public void SetUp()
        {
            this.dishes = new List<Dish>();
            this.dishCategories = new List<DishCategory>();

            MockRepositoryMethods();
            MockDishCategoryServiceMethods();
        }

        private void MockDishCategoryServiceMethods()
        {
            this.dishCategoryServiceMock
                .Setup(x => x.GetByIdOrDefault(It.IsAny<int>()))
                .Returns<int>(async x =>
                {
                    var dishCategoryServiceModel =
                        this.mapper.Map<DishCategoryServiceModel>(this.dishCategories.First(a => a.Id == x));
                    return await Task.FromResult(dishCategoryServiceModel);
                });

            this.dishCategoryServiceMock
                .Setup(x => x.IsItemExistInCategory(It.IsAny<int>(), It.IsAny<string>()))
                .Returns<int, string>(async (categoryId, name) =>
                {
                    var isDishExist = this.dishes.FirstOrDefault(x => x.CategoryId == categoryId && x.Name == name);

                    return isDishExist != null
                        ? await Task.FromResult(true)
                        : await Task.FromResult(false);
                });
        }

        private void MockRepositoryMethods()
        {
            this.dishRepoMock
                .Setup(x => x.FindOrDefaultAsync(It.IsAny<Expression<Func<Dish, bool>>>()))
                .Returns<Expression<Func<Dish, bool>>>(async x => await Task.FromResult(this.dishes.FirstOrDefault(x.Compile())));

            this.dishRepoMock
                .Setup(x => x.GetDishWithCategoryById(It.IsAny<string>()))
                .Returns<string>(async x => await Task.FromResult(this.dishes.First(a => a.Id == x)));

            this.dishRepoMock
                .Setup(x => x.AddAsync(It.IsAny<Dish>()))
                .Callback<Dish>(x => this.dishes.Add(x));

            this.dishRepoMock
                .Setup(x => x.Update(It.IsAny<Dish>()))
                .Callback<Dish>(x =>
                {
                    var dishFromStorage = this.dishes.First(a => a.Id == x.Id);

                    dishFromStorage.Name = x.Name;
                    dishFromStorage.ImageUrl = x.ImageUrl;
                    dishFromStorage.CategoryId = x.CategoryId;
                    dishFromStorage.Price = x.Price;
                    dishFromStorage.Description = x.Description;
                    dishFromStorage.Quantity = x.Quantity;
                });
        }

        private void SeedTestDishes()
        {
            var soupsCategory = new DishCategory() { Id = 1, Title = "Soups"};
            var pizzaCategory = new DishCategory() { Id = 2, Title = "Pizza", IsDisable = true };
            var bbqCategory = new DishCategory() { Id = 3, Title = "BBQ" };
            var salads = new DishCategory() { Id = 4, Title = "Salads", IsDisable = true };

            this.dishCategories.Add(soupsCategory);
            this.dishCategories.Add(pizzaCategory);
            this.dishCategories.Add(bbqCategory);
            this.dishCategories.Add(salads);

            this.dishes.Add(new()
            {
                Id = "c3182fae-c620-4b79-be5c-0f05e104f9ea",
                Name = "Chicken soup",
                ImageUrl = "chickensoup.jpg",
                CategoryId = 1,
                Category = soupsCategory,
                Description = "chicken soup test",
                Price = 12.20M,
                Quantity = 20
            });
            this.dishes.Add(new()
            {
                Id = "82496e1f-3120-4016-860e-e98558678477",
                Name = "Sweet soup",
                ImageUrl = "sweet soup",
                CategoryId = 1,
                Category = soupsCategory,
                Description = "sweet soup",
                Price = 15.40M,
                Quantity = 1
            });
            this.dishes.Add(new()
            {
                Id = "81ba5e61-6ed8-454b-8419-67ebd4f16e74",
                Name = "Pizza",
                ImageUrl = "pizza.jpg",
                CategoryId = 2,
                Category = pizzaCategory,
                Description = "pizza test",
                Price = 15.20M,
                Quantity = 10
            });
            this.dishes.Add(new()
            {
                Id = "5bc5fdbc-4bba-46fa-aa18-1f307a1d48ae",
                Name = "Steak",
                ImageUrl = "steak.jpg",
                CategoryId = 3,
                Category = bbqCategory,
                Description = "steak test",
                Price = 16.20M,
                Quantity = 3
            });
            this.dishes.Add(new()
            {
                Id = "38db3486-ea3b-44cb-8d27-8a68fd84ae30",
                Name = "Greek Salad",
                ImageUrl = "salad.jpg",
                CategoryId = 4,
                Category = salads,
                Description = "salad test",
                Price = 9.20M,
                Quantity = 14
            });
        }

        [Test]
        public void When_IsCreatedObject_PassedParameter_ShouldBeSetted()
        {
            var dishServiceType = this.dishService.GetType();

            var dishRepoFieldValue = dishServiceType
                .GetField("dishRepository", BindingFlags.NonPublic | BindingFlags.Instance)
                .GetValue(this.dishService);

            var dishCategoryServiceFieldValue = dishServiceType
                .GetField("dishCategoryService", BindingFlags.NonPublic | BindingFlags.Instance)
                .GetValue(this.dishService);

            var mapperFieldValue = dishServiceType
                .GetField("mapper", BindingFlags.NonPublic | BindingFlags.Instance)
                .GetValue(this.dishService);

            Assert.AreEqual(this.dishRepoMock.Object, dishRepoFieldValue);
            Assert.AreEqual(this.dishCategoryServiceMock.Object, dishCategoryServiceFieldValue);
            Assert.AreEqual(this.mapper, mapperFieldValue);
        }

        [Test]
        [TestCase("c3182fae-c620-4b79-be5c-0f05e104f9ea")]
        [TestCase("81ba5e61-6ed8-454b-8419-67ebd4f16e74")]
        [TestCase("5bc5fdbc-4bba-46fa-aa18-1f307a1d48ae")]
        [TestCase("38db3486-ea3b-44cb-8d27-8a68fd84ae30")]
        public async Task When_CallGetByIdOrDefaultWithValidId_ShouldReturnDish(string id)
        {
            SeedTestDishes();

            var actualResult = await this.dishService
                .GetByIdOrDefault(id);

            var expectedResult = this.dishes.First(x => x.Id == id);

            Assert.AreEqual(expectedResult.Id, actualResult.Id);
            Assert.AreEqual(expectedResult.Name, actualResult.Name);
            Assert.AreEqual(expectedResult.ImageUrl, actualResult.ImageUrl);
            Assert.AreEqual(expectedResult.CategoryId, actualResult.Category.Id);
            Assert.AreEqual(expectedResult.Category.Id, actualResult.Category.Id);
            Assert.AreEqual(expectedResult.Category.Title, actualResult.Category.Title);
            Assert.AreEqual(expectedResult.Price, actualResult.Price);
            Assert.AreEqual(expectedResult.Description, actualResult.Description);
            Assert.AreEqual(expectedResult.Quantity, actualResult.Quantity);
        }

        [Test]
        [TestCase("c3182fae-c620-4b79-be5c-0f05e104f9e1")]
        [TestCase("81ba5e61-6ed8-454b-8419-67ebd4f16e7a")]
        [TestCase("5bc5fdbc-4bba-46fa-aa18-1f307a1d48a2")]
        [TestCase("38db3486-ea3b-44cb-8d27-8a68fd84ae3e")]
        public async Task When_CallGetByIdOrDefaultWithInValidId_ShouldReturnNull(string id)
        {
            SeedTestDishes();

            var actualResult = await this.dishService
                .GetByIdOrDefault(id);

            Assert.IsNull(actualResult);
        }

        [Test]
        [TestCase("Chicken soup", "chicken.jpg", 1, 12.20, "chicken soup test", 120ul)]
        [TestCase("Pizza", "pizza.jpg", 2, 10.20, "pizza test", 10ul)]
        [TestCase("Steak", "steak.jpg", 5, 22.20, "steak test", 25ul)]
        [TestCase("Greek Salad", "salad.jpg", 3, 9.20, "greek salad test", 40ul)]
        public async Task When_CallAdd_DishShouldBeAddedToStore(
            string name,
            string imageUrl,
            int categoryId,
            decimal price,
            string description,
            ulong quantity)
        {
            await this.dishService
                .Add(name,
                    imageUrl,
                    categoryId,
                    price,
                    description,
                    quantity);

            var actualResult = this.dishes.FirstOrDefault(x => x.Name == name);

            Assert.AreEqual(1, this.dishes.Count);

            Assert.AreEqual(name, actualResult.Name);
            Assert.AreEqual(imageUrl, actualResult.ImageUrl);
            Assert.AreEqual(categoryId, actualResult.CategoryId);
            Assert.AreEqual(price, actualResult.Price);
            Assert.AreEqual(description, actualResult.Description);
            Assert.AreEqual(quantity, actualResult.Quantity);
        }

        [Test]
        [TestCase("c3182fae-c620-4b79-be5c-0f05e104f9ea", "update chicken soup", "chicken.jpg", 2, 12.20, "test update", 1ul)]
        [TestCase("81ba5e61-6ed8-454b-8419-67ebd4f16e74", "update pizza", "pizza.jpg", 3, 14.20, "test pizza", 2ul)]
        public async Task When_CallUpdate_ShouldUpdateDishInStore(
            string id,
            string name,
            string imageUrl,
            int categoryId,
            decimal price,
            string description,
            ulong quantity)
        {
            SeedTestDishes();

            var result = await this.dishService
                .Update(id, name, imageUrl, categoryId, price, description, quantity);

            var actualResult = this.dishes.First(x => x.Id == id);

            Assert.IsTrue(result);

            Assert.AreEqual(id, actualResult.Id);
            Assert.AreEqual(name, actualResult.Name);
            Assert.AreEqual(imageUrl, actualResult.ImageUrl);
            Assert.AreEqual(categoryId, actualResult.CategoryId);
            Assert.AreEqual(price, actualResult.Price);
            Assert.AreEqual(description, actualResult.Description);
            Assert.AreEqual(quantity, actualResult.Quantity);
        }

        [Test]
        [TestCase("c3182fae-c620-4b79-be5c-0f05e104f9ea", "Pizza", "pizza.jpg", 2, 20.22, "test update", 12ul)]
        [TestCase("82496e1f-3120-4016-860e-e98558678477", "Pizza", "pizza.jpg", 2, 12.22, "test update pizza", 14ul)]
        [TestCase("c3182fae-c620-4b79-be5c-0f05e104f9ea", "Steak", "sweet.jpg", 3, 12.40, "test steak", 15ul)]
        public async Task When_CallUpdate_WithAlreadyExistingDishInAnotherCategory_ShouldReturnFalseAndNotUpdateDish(
            string id,
            string name,
            string imageUrl,
            int categoryId,
            decimal price,
            string description,
            ulong quantity)
        {
            SeedTestDishes();

            var resultFromMethod = await this.dishService
                .Update(id, name, imageUrl, categoryId, price, description, quantity);

            var expectedResult = this.dishes.FirstOrDefault(x => x.Id == id); 

            Assert.IsFalse(resultFromMethod);

            Assert.AreNotEqual(expectedResult.Name, name);
            Assert.AreNotEqual(expectedResult.ImageUrl, imageUrl);
            Assert.AreNotEqual(expectedResult.CategoryId, categoryId);
            Assert.AreNotEqual(expectedResult.Price, price);
            Assert.AreNotEqual(expectedResult.Description, description);
            Assert.AreNotEqual(expectedResult.Quantity, quantity);
        }

        [Test]
        [TestCase("82496e1f-3120-4016-860e-e98558678477", "Chicken soup", "chicken.jpg", 1, 15.30, "test chicken", 13ul)]
        [TestCase("c3182fae-c620-4b79-be5c-0f05e104f9ea", "Sweet soup", "soup.jpg", 1, 120.20, "test soup", 100ul)]
        public async Task When_CallUpdate_WithAlreadyExistingDishInSameCategory_ShouldReturnFalseAndNotUpdateDish(
            string id,
            string name,
            string imageUrl,
            int categoryId,
            decimal price,
            string description,
            ulong quantity)
        {
            SeedTestDishes();

            var resultFromMethod = await this.dishService
                .Update(id, name, imageUrl, categoryId, price, description, quantity);

            var expectedResult = this.dishes.FirstOrDefault(x => x.Id == id);

            Assert.IsFalse(resultFromMethod);

            Assert.AreNotEqual(expectedResult.Name, name);
            Assert.AreNotEqual(expectedResult.ImageUrl, imageUrl);
            Assert.AreEqual(expectedResult.CategoryId, categoryId);
            Assert.AreNotEqual(expectedResult.Price, price);
            Assert.AreNotEqual(expectedResult.Description, description);
            Assert.AreNotEqual(expectedResult.Quantity, quantity);
        }
    }
}
