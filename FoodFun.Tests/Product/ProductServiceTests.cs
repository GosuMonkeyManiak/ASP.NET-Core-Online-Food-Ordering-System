namespace FoodFun.Tests.Product
{
    using AutoMapper;
    using Core.AutoMapper;
    using Core.Contracts;
    using Core.Models.ProductCategory;
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
    using System.Threading.Tasks;

    [TestFixture]
    public class ProductServiceTests
    {
        private IProductService productService;
        private Mock<IProductRepository> productRepoMock;
        private Mock<IProductCategoryService> productCategoryServiceMock;
        private IMapper mapper;

        private IList<Product> products;
        private IList<ProductCategory> productCategories;
        
        private readonly IServiceProvider serviceProvider;

        public ProductServiceTests()
        {
            IServiceCollection services = new ServiceCollection()
                .AddAutoMapper(typeof(AutoMapperServiceProfile));
            this.serviceProvider = services.BuildServiceProvider();

            this.mapper = this.serviceProvider.GetService<IMapper>();
        }

        [SetUp]
        public void SetUp()
        {
            this.productRepoMock = new Mock<IProductRepository>();
            this.productCategoryServiceMock = new Mock<IProductCategoryService>();

            this.productService =
                new ProductService(
                    this.productRepoMock.Object,
                    this.productCategoryServiceMock.Object,
                    this.mapper);

            this.products = new List<Product>();
            this.productCategories = new List<ProductCategory>();

            MockProductCategoryServiceMethods();
            MockProductRepositoryMethods();
        }

        private void MockProductRepositoryMethods()
        {
            this.productRepoMock
                .Setup(x => x.FindOrDefaultAsync(It.IsAny<Expression<Func<Product, bool>>>()))
                .Returns<Expression<Func<Product, bool>>>(async x => await Task.FromResult(this.products.FirstOrDefault(x.Compile())));

            this.productRepoMock
                .Setup(x => x.GetProductWithCategoryById(It.IsAny<string>()))
                .Returns<string>(async x => await Task.FromResult(this.products.First(a => a.Id == x)));

            this.productRepoMock
                .Setup(x => x.Update(It.IsAny<Product>()))
                .Callback<Product>(x =>
                {
                    var product = this.products.FirstOrDefault(a => a.Id == x.Id);

                    product.Name = x.Name;
                    product.ImageUrl = x.ImageUrl;
                    product.CategoryId = x.CategoryId;
                    product.Price = x.Price;
                    product.Description = x.Description;
                    product.Quantity = x.Quantity;
                });

            this.productRepoMock
                .Setup(x => x.GetCountOfProductsByFilters(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<bool>()))
                .Returns<string, int, bool>(async (searchTerm, categoryFilterId, onlyAvailable) =>
                {
                    if (searchTerm != null)
                    {
                        this.products = this.products
                            .Where(x => x.Name.Contains(searchTerm) || x.Description.Contains(searchTerm))
                            .ToList();
                    }

                    if (categoryFilterId != 0)
                    {
                        this.products = this.products
                            .Where(x => x.CategoryId == categoryFilterId)
                            .ToList();
                    }

                    if (onlyAvailable)
                    {
                        this.products = this.products
                            .Where(x => !x.Category.IsDisable)
                            .Where(x => x.Quantity > 0)
                            .ToList();
                    }

                    return this.products.Count;
                });

            this.productRepoMock
                .Setup(x => x.GetAllProductsWithCategories(
                    It.IsAny<string>(),
                    It.IsAny<int>(),
                    It.IsAny<byte>(),
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<bool>()))
                .Returns<string, int, byte, int, int, bool>(
                    async (searchTerm, categoryFilterId, orderNumber, pageNumber, pageSize, onlyAvailable) =>
                    {
                        if (searchTerm != null)
                        {
                            this.products = this.products
                                .Where(x => x.Name.Contains(searchTerm) || x.Description.Contains(searchTerm))
                                .ToList();
                        }

                        if (categoryFilterId != 0)
                        {
                            this.products = this.products
                                .Where(x => x.CategoryId == categoryFilterId)
                                .ToList();
                        }

                        if (orderNumber == 1)
                        {
                            this.products = products
                                .OrderByDescending(x => x.Price)
                                .ToList();
                        }
                        else
                        {
                            this.products = products
                                .OrderBy(x => x.Price)
                                .ToList();
                        }

                        if (onlyAvailable)
                        {
                            this.products = this.products
                                .Where(x => !x.Category.IsDisable)
                                .Where(x => x.Quantity > 0)
                                .ToList();
                        }

                        return await Task.FromResult(this.products
                            .Skip((pageNumber - 1) * pageSize)
                            .Take(pageSize)
                            .ToList());
                    });
        }

        private void MockProductCategoryServiceMethods()
        {
            this.productCategoryServiceMock
                .Setup(x => x.IsCategoryExist(It.IsAny<int>()))
                .Returns<int>(async x => await Task.FromResult(this.products.Any(a => a.CategoryId == x)));

            this.productCategoryServiceMock
                .Setup(x => x.GetByIdOrDefault(It.IsAny<int>()))
                .Returns<int>(async x =>
                {
                    var category = this.productCategories.First(a => a.Id == x);
                    return await Task.FromResult(mapper.Map<ProductCategoryServiceModel>(category));
                });

            this.productCategoryServiceMock
                .Setup(x => x.IsProductExistInCategory(It.IsAny<int>(), It.IsAny<string>()))
                .Returns<int, string>(async (id, name) =>
                {
                    var isProductExist = this.products.FirstOrDefault(x => x.CategoryId == id && x.Name == name);

                    return isProductExist != null
                        ? await Task.FromResult(true)
                        : await Task.FromResult(false);
                });
        }

        private MethodInfo GetValidationMethodFromProductService()
            => this.productService
                .GetType()
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

             return await (Task<Tuple<string, int, byte, int>>) validateSearchParametersMethod
                .Invoke(this.productService,
                    new object?[] { searchTerm, categoryFilterId, orderNumber, pageNumber, pageSize, onlyAvailable });
        }

        private void SeedTestProductsAndCategories()
        {
            var fruitCategory = new ProductCategory() { Id = 1, Title = "Fruits" };
            var vegetablesCategory = new ProductCategory() { Id = 2,Title = "Vegetables", IsDisable = true };
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

        [Test]
        public void When_CreatingProductServiceFields_ShouldBeSet()
        {
            var productServiceType = this.productService
                .GetType();

            var valueOfProductRepositoryField = productServiceType
                .GetField("productRepository", BindingFlags.NonPublic | BindingFlags.Instance)
                .GetValue(this.productService);

            var valueOfProductCategoryServiceField = productServiceType
                .GetField("productCategoryService", BindingFlags.NonPublic | BindingFlags.Instance)
                .GetValue(this.productService);

            var valueOfMapperField = productServiceType
                .GetField("mapper", BindingFlags.NonPublic | BindingFlags.Instance)
                .GetValue(this.productService);

            Assert.AreEqual(this.productRepoMock.Object, valueOfProductRepositoryField);
            Assert.AreEqual(this.productCategoryServiceMock.Object, valueOfProductCategoryServiceField);
            Assert.AreEqual(this.mapper, valueOfMapperField);
        }

        [Test]
        [TestCase("Bananas", "bananas.jpg", 1, 10.10, "Test bananas", 120)]
        [TestCase("Apples", "apples.jpg", 2, 11, "Test apples", 300)]
        [TestCase("Oranges", "oranges.jpg", 5, 1.20, "Test oranges", 1)]
        [TestCase("WaterMelon", "waterMelon.jpg", 10, 12.02, "Test watermelon", 0)]
        public async Task When_ProvideProductInfo_ShouldBeCreated(
            string name,
            string imageUrl,
            int categoryId,
            decimal price,
            string description,
            long quantity)
        {
            this.productRepoMock
                .Setup(x => x.AddAsync(It.IsAny<Product>()))
                .Callback<Product>(x => this.products.Add(x));

            await this.productService
                .Add(name, imageUrl, categoryId, price, description, quantity);

            var createdProduct = this.products.First(x => x.Name == name);

            var productsCount = this.products.Count;

            Assert.NotNull(createdProduct);
            Assert.AreEqual(1, productsCount);

            Assert.AreEqual(name, createdProduct.Name);
            Assert.AreEqual(imageUrl, createdProduct.ImageUrl);
            Assert.AreEqual(categoryId, createdProduct.CategoryId);
            Assert.AreEqual(price, createdProduct.Price);
            Assert.AreEqual(description, createdProduct.Description);
            Assert.AreEqual(quantity, createdProduct.Quantity);
        }

        [Test]
        [TestCase("4da54227-ccf1-4fd5-9fb5-21ae4356da33")]
        [TestCase("999cae77-6db9-4437-bb4f-440bcfcc8772")]
        [TestCase("22164c80-b0de-4633-ada5-a74ac0674843")]
        public async Task When_ProvideValidProductId_ShouldReturnProduct(string id)
        {
            SeedTestProductsAndCategories();

            var productFromService = await this.productService
                .GetByIdOrDefault(id);

            var actualProduct = this.products.First(x => x.Id == id);

            Assert.IsNotNull(productFromService);
            Assert.AreEqual(actualProduct.Id, productFromService.Id);
            Assert.AreEqual(actualProduct.Name, productFromService.Name);
            Assert.AreEqual(actualProduct.ImageUrl, productFromService.ImageUrl);
            Assert.AreEqual(actualProduct.CategoryId, productFromService.Category.Id);
            Assert.AreEqual(actualProduct.Category.Id, productFromService.Category.Id);
            Assert.AreEqual(actualProduct.Category.Title, productFromService.Category.Title);
            Assert.AreEqual(actualProduct.Price, productFromService.Price);
            Assert.AreEqual(actualProduct.Description, productFromService.Description);
            Assert.AreEqual(actualProduct.Quantity, productFromService.Quantity);
        }

        [Test]
        [TestCase("4da54227-ccf1-4fd5-9fb5-21ae4356da3a")]
        [TestCase("999cae77-6db9-4437-bb4f-440bcfcc8771")]
        [TestCase("22164c80-b0de-4633-ada5-a74ac067484b")]
        public async Task When_ProvideInValidProductId_ShouldReturnNull(string id)
        {
            SeedTestProductsAndCategories();

            var productFromService = await this.productService
                .GetByIdOrDefault(id);

            Assert.IsNull(productFromService);
        }

        [Test]
        [TestCase("4da54227-ccf1-4fd5-9fb5-21ae4356da33", "WaterMelon", "waterMelon.jpg", 1, 12.20, "Test waterMelon", 12)]
        [TestCase("4da54227-ccf1-4fd5-9fb5-21ae4356da33", "Tomatos", "tomatos.jpg", 2, 12.20, "Test tomatos", 12)]
        public async Task When_TryUpdateProductAndAlreadyProductWithThatInfoExist_ShouldReturnFalse(
            string id,
            string name,
            string imageUrl,
            int categoryId,
            decimal price,
            string description,
            long quantity)
        {
            SeedTestProductsAndCategories();

           var result = await this.productService
                .Update(id, name, imageUrl, categoryId, price, description, quantity);

           Assert.AreEqual(false, result);
        }

        [Test]
        [TestCase("4da54227-ccf1-4fd5-9fb5-21ae4356da33", "Fish", "fish.jpg", 3, 120, "Test fish", 12)]
        [TestCase("058e7d03-7082-4d92-9fa3-b0458afd484f", "Melon", "melon.jpg", 2, 100, "Test melon", 3)]
        [TestCase("999cae77-6db9-4437-bb4f-440bcfcc8772", "Tomatos", "tomatos.jpg", 3, 121, "Test tomatos", 4)]
        public async Task When_UpdateProduct_ShouldReturnTrueAndUpdateProduct(
            string id,
            string name,
            string imageUrl,
            int categoryId,
            decimal price,
            string description,
            long quantity)
        {
            SeedTestProductsAndCategories();

            var result = await this.productService
                .Update(id, name, imageUrl, categoryId, price, description, quantity);

            var changedProduct = this.products.First(x => x.Id == id);

            Assert.AreEqual(true, result);
            Assert.AreEqual(id, changedProduct.Id);
            Assert.AreEqual(name, changedProduct.Name);
            Assert.AreEqual(imageUrl, changedProduct.ImageUrl);
            Assert.AreEqual(categoryId, changedProduct.CategoryId);
            Assert.AreEqual(price, changedProduct.Price);
            Assert.AreEqual(description, changedProduct.Description);
            Assert.AreEqual(quantity, changedProduct.Quantity);
        }

        [Test]
        [TestCase(null, 2, 3, true)]
        [TestCase(null, 2, 3, false)]
        [TestCase("ba", 1, 1, true)]
        [TestCase("ka", 3, 1, false)]
        [TestCase("a", 3, 1, false)]
        public async Task When_PopulateLastPageNumberWithFiltersWhichExist_ShouldLasPageNumberProperty_ToHaveValueBiggerThanZero(
            string searchTerm,
            int categoryFilterId,
            int pageSize,
            bool onlyAvailable)
        {
            SeedTestProductsAndCategories();

            var productServiceType = this.productService
                .GetType();

            var method = productServiceType
                .GetMethod("PopulateLastPageNumberByFilter", BindingFlags.NonPublic | BindingFlags.Instance);

            method.Invoke(this.productService, new object?[] { searchTerm, categoryFilterId, pageSize, onlyAvailable});

            var actualLastPageNumberValue = (int) productServiceType
                .GetProperty("LastPageNumber", BindingFlags.NonPublic | BindingFlags.Instance)
                .GetValue(this.productService);

            var countOfProductsByFilters = await this.productRepoMock
                .Object
                .GetCountOfProductsByFilters(
                    searchTerm,
                    categoryFilterId,
                    onlyAvailable);

            var expectedLastPageNumberValue = (int) Math.Ceiling(countOfProductsByFilters / (pageSize * 1.0));

            Assert.AreEqual(expectedLastPageNumberValue, actualLastPageNumberValue);
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
        [TestCase(null, 120, 2, 22, 1, true)]
        [TestCase(null, 1201, 3, 320, 2, false)]
        [TestCase("ba ba", 1, 0, 1, 2, true)]
        [TestCase("ba ba", 1, 0, 1, 2, false)]
        public async Task When_PassSearchParameters_ShouldReturnProperFilteredProducts(
            string searchTerm,
            int categoryFilterId,
            byte orderNumber,
            int pageNumber,
            int pageSize,
            bool onlyAvailable)
        {
            SeedTestProductsAndCategories();

            var (FilteredProducts,
                actualCurrentPage,
                actualLastPage,
                actualSelectedCategoryId) = await this.productService.All(
                    searchTerm,
                    categoryFilterId,
                    orderNumber,
                    pageNumber,
                    pageSize,
                    onlyAvailable);

            var (validSearchTerm,
                validCategoryFilterId,
                validOrderNumber,
                validPageNumber) = await GetResultFromInvokeValidationMethod(
                    searchTerm,
                    categoryFilterId,
                    orderNumber,
                    pageNumber,
                    pageSize,
                    onlyAvailable);

            var expectedFilteredProducts = (await this.productRepoMock.Object.GetAllProductsWithCategories(
                    validSearchTerm,
                    validCategoryFilterId,
                    validOrderNumber,
                    validPageNumber,
                    pageSize,
                    onlyAvailable))
                .ToList();

            var actualFilteredProducts = FilteredProducts.ToList();

            Assert.AreEqual(expectedFilteredProducts.Count, actualFilteredProducts.Count);

            for (int i = 0; i < expectedFilteredProducts.Count; i++)
            {
                Assert.AreEqual(expectedFilteredProducts[i].Id, actualFilteredProducts[i].Id);
                Assert.AreEqual(expectedFilteredProducts[i].Name, actualFilteredProducts[i].Name);
                Assert.AreEqual(expectedFilteredProducts[i].ImageUrl, actualFilteredProducts[i].ImageUrl);
                Assert.AreEqual(expectedFilteredProducts[i].CategoryId, actualFilteredProducts[i].Category.Id);
                Assert.AreEqual(expectedFilteredProducts[i].Category.Id, actualFilteredProducts[i].Category.Id);
                Assert.AreEqual(expectedFilteredProducts[i].Category.Title, actualFilteredProducts[i].Category.Title);
                Assert.AreEqual(expectedFilteredProducts[i].Price, actualFilteredProducts[i].Price);
                Assert.AreEqual(expectedFilteredProducts[i].Description, actualFilteredProducts[i].Description);
                Assert.AreEqual(expectedFilteredProducts[i].Quantity, actualFilteredProducts[i].Quantity);
            }
        }
    }
}
