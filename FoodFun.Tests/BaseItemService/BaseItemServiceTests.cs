namespace FoodFun.Tests.BaseItemService
{
    using AutoMapper;
    using Core.Contracts;
    using Core.Services;
    using Core.Services.Base;
    using Infrastructure.Common.Contracts;
    using Infrastructure.Models;
    using Moq;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    [TestFixture]
    public class BaseItemServiceTests
    {
        private BaseItemService productService;

        private Mock<IProductRepository> productRepository;
        private Mock<IProductCategoryService> productCategoryService;
        private Mock<IMapper> mapper;

        private IList<Product> products;
        private IList<ProductCategory> productCategories;

        public BaseItemServiceTests()
        {
            this.productRepository = new Mock<IProductRepository>();
            this.productCategoryService = new Mock<IProductCategoryService>();
            this.mapper = new Mock<IMapper>();

            this.productService = new ProductService(
                this.productRepository.Object,
                this.productCategoryService.Object,
                this.mapper.Object);
        }

        [SetUp]
        public void Setup()
        {
            products = new List<Product>();
            productCategories = new List<ProductCategory>();

            MockRepoMethod();
            MockProductCategoryServiceMethods();
        }

        private void MockRepoMethod()
        {
            this.productRepository
                .Setup(x => x.GetCountOfItemsByFilters(
                    It.IsAny<string>(),
                    It.IsAny<int>(),
                    It.IsAny<bool>()))
                .Returns<string, int, bool>(async (searchTerm, categoryFilterId, onlyAvailable) =>
                {
                    var result = new List<Product>();

                    if (searchTerm != null)
                    {
                       result = this.products
                            .Where(x => x.Name.Contains(searchTerm)
                                        || x.Description.Contains(searchTerm))
                            .ToList();
                    }

                    if (categoryFilterId != 0)
                    {
                        result = this.products
                            .Where(x => x.CategoryId == categoryFilterId)
                            .ToList();
                    }

                    if (onlyAvailable)
                    {
                        result = this.products
                            .Where(x => !x.Category.IsDisable)
                            .ToList();
                    }

                    return await Task.FromResult(result.Count);
                });
        }

        private void MockProductCategoryServiceMethods()
        {
            this.productCategoryService
                .Setup(x => x.IsCategoryExist(It.IsAny<int>()))
                .Returns<int>(async x => await Task.FromResult(this.productCategories.FirstOrDefault(a => a.Id == x) != null));
        }

        private void SeedTestProductsAndCategories()
        {
            var fruitCategory = new ProductCategory() { Id = 1, Title = "Fruits" };
            var vegetablesCategory = new ProductCategory() { Id = 2, Title = "Vegetables", IsDisable = true };
            var meatCategory = new ProductCategory() { Id = 3, Title = "Meat" };

            this.productCategories.Add(fruitCategory);
            this.productCategories.Add(vegetablesCategory);
            this.productCategories.Add(meatCategory);

            this.products.Add(new()
            {
                Id = "4da54227-ccf1-4fd5-9fb5-21ae4356da33",
                Name = "Bananas",
                ImageUrl = "bananas.jpg",
                CategoryId = fruitCategory.Id,
                Category = fruitCategory,
                Price = 12.20M,
                Description = "Test bananas",
                Quantity = 120
            });
            this.products.Add(new()
            {
                Id = "058e7d03-7082-4d92-9fa3-b0458afd484f",
                Name = "WaterMelon",
                ImageUrl = "waterMelon.jpg",
                CategoryId = fruitCategory.Id,
                Category = fruitCategory,
                Price = 10.20M,
                Description = "Test waterMelon",
                Quantity = 40
            });
            this.products.Add(new()
            {
                Id = "999cae77-6db9-4437-bb4f-440bcfcc8772",
                Name = "Tomatos",
                ImageUrl = "tomatos.jpg",
                CategoryId = vegetablesCategory.Id,
                Category = vegetablesCategory,
                Price = 9.20M,
                Description = "Test tomatos",
                Quantity = 12
            });
            this.products.Add(new()
            {
                Id = "22164c80-b0de-4633-ada5-a74ac0674843",
                Name = "Chicken",
                ImageUrl = "chicken.jpg",
                CategoryId = meatCategory.Id,
                Category = meatCategory,
                Price = 12,
                Description = "Test chicken",
                Quantity = 0
            });
        }

        private MethodInfo GetValidationMethodFromProductService()
            => this.productService
                .GetType()
                .BaseType
                .GetMethod(
                    "ValidateAndSetDefaultSearchParameters",
                    BindingFlags.NonPublic | BindingFlags.Instance);

        private async Task<Tuple<string, int, byte, int>> GetResultFromInvokeValidationMethod(
            string searchTerm,
            int categoryFilterId,
            byte orderNumber,
            int pageNumber,
            int pageSize,
            bool onlyAvailable)
        {
            var validateSearchParametersMethod = GetValidationMethodFromProductService();

            return await (Task<Tuple<string, int, byte, int>>)validateSearchParametersMethod
               .Invoke(this.productService,
                   new object?[] { 
                       searchTerm, 
                       categoryFilterId, 
                       orderNumber, 
                       pageNumber, 
                       pageSize, 
                       onlyAvailable, 
                       this.productCategoryService.Object,
                       this.productRepository.Object
                   });
        }

        [Test]
        [TestCase("ba ba", 1, 1, 1, 2, true)]
        [TestCase("bana", 2, 0, 1, 2, false)]
        [TestCase("a", 2, 0, 1, 2, false)]
        public async Task When_PassValidSearchParameters_ShouldBeSetted(
            string searchTerm,
            int categoryFilterId,
            byte orderNumber,
            int pageNumber,
            int pageSize,
            bool onlyAvailable)
        {
            SeedTestProductsAndCategories();

            var (actualSearchTermResult,
                actualCategoryIdResult,
                actualOrderNumberResult,
                actualPageNumberResult) = await GetResultFromInvokeValidationMethod(
                    searchTerm,
                    categoryFilterId,
                    orderNumber,
                    pageNumber,
                    pageSize,
                    onlyAvailable);

            Assert.AreEqual(searchTerm, actualSearchTermResult);
            Assert.AreEqual(categoryFilterId, actualCategoryIdResult);
            Assert.AreEqual(orderNumber, actualOrderNumberResult);
            Assert.AreEqual(pageNumber, actualPageNumberResult);
        }

        [Test]
        [TestCase(null, 120, 2, 121, 120, true)]
        [TestCase(null, 120, 2, 121, 120, false)]
        [TestCase(null, 50, 120, 20, 153, true)]
        [TestCase(null, 50, 120, 20, 153, false)]
        public async Task When_PassInvalidSearchParameters_ShouldReturnDefault(
            string searchTerm,
            int categoryFilterId,
            byte orderNumber,
            int pageNumber,
            int pageSize,
            bool onlyAvailable)
        {
            SeedTestProductsAndCategories();

            var (actualSearchTermResult,
                actualCategoryIdResult,
                actualOrderNumberResult,
                actualPageNumberResult) = await GetResultFromInvokeValidationMethod(
                    searchTerm,
                    categoryFilterId,
                    orderNumber,
                    pageNumber,
                    pageSize,
                    onlyAvailable);

            string expectedSearchTermResult = null;
            var expectedCategoryIdResult = 0;
            var expectedOrderNumberResult = 0;
            var expectedPageNumberResult = 1;

            Assert.AreEqual(expectedSearchTermResult, actualSearchTermResult);
            Assert.AreEqual(expectedCategoryIdResult, actualCategoryIdResult);
            Assert.AreEqual(expectedOrderNumberResult, actualOrderNumberResult);
            Assert.AreEqual(expectedPageNumberResult, actualPageNumberResult);
        }

        [Test]
        [TestCase(null, 2, 3, true)]
        [TestCase(null, 2, 3, false)]
        [TestCase("ba", 1, 1, true)]
        [TestCase("ka", 3, 1, false)]
        [TestCase("a", 3, 1, false)]
        public async Task When_PopulateLastPageNumberWithFiltersWhichExist_ShouldLastPageNumberProperty_ToHaveValueBiggerThanZero(
            string searchTerm,
            int categoryFilterId,
            int pageSize,
            bool onlyAvailable)
        {
            SeedTestProductsAndCategories();

            var baseItemServiceType = this.productService
                .GetType()
                .BaseType;

            var method = baseItemServiceType
                .GetMethod("PopulateLastPageNumberByFilter", BindingFlags.NonPublic | BindingFlags.Instance);

            method.Invoke(this.productService, new object?[] { searchTerm, categoryFilterId, pageSize, onlyAvailable, this.productRepository.Object });

            var actualLastPageNumberValue = (int)baseItemServiceType
                .GetProperty("LastPageNumber", BindingFlags.NonPublic | BindingFlags.Instance)
                .GetValue(this.productService);

            var countOfProductsByFilters = await this.productRepository
                .Object
                .GetCountOfItemsByFilters(
                    searchTerm,
                    categoryFilterId,
                    onlyAvailable);

            var expectedLastPageNumberValue = (int)Math.Ceiling(countOfProductsByFilters / (pageSize * 1.0));

            Assert.AreEqual(expectedLastPageNumberValue, actualLastPageNumberValue);
        }
    }
}
