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

            this.productRepoMock
                .Setup(x => x.FindOrDefaultAsync(It.IsAny<Expression<Func<Product, bool>>>()))
                .Returns<Expression<Func<Product, bool>>>(async x => await Task.FromResult(this.products.FirstOrDefault(x.Compile())));

            this.productRepoMock
                .Setup(x => x.GetProductWithCategoryById(It.IsAny<string>()))
                .Returns<string>(async x => await Task.FromResult(this.products.First(a => a.Id == x)));

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
        }

        private void SeedTestProductsAndCategories()
        {
            var fruitCategory = new ProductCategory() { Id = 1, Title = "Fruits"};
            var vegetablesCategory = new ProductCategory() { Id = 2,Title = "Vegetables" };
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
    }
}
