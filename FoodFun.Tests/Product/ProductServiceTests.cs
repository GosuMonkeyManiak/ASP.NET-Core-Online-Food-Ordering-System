namespace FoodFun.Tests.Product
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using AutoMapper;
    using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
    using Core.AutoMapper;
    using Core.Contracts;
    using Core.Models.Product;
    using Core.Models.ProductCategory;
    using Core.Services;
    using Infrastructure.Common.Contracts;
    using Infrastructure.Models;
    using Microsoft.Extensions.DependencyInjection;
    using Moq;
    using NUnit.Framework;
    using Expression = Castle.DynamicProxy.Generators.Emitters.SimpleAST.Expression;

    [TestFixture]
    public class ProductServiceTests
    {
        private IProductService productService;
        private Mock<IProductRepository> productRepoMock;
        private Mock<IProductCategoryService> productCategoryServiceMock;
        private IList<Product> products;
        private readonly IServiceProvider serviceProvider;

        public ProductServiceTests()
        {
            IServiceCollection services = new ServiceCollection()
                .AddAutoMapper(typeof(AutoMapperServiceProfile));

            this.serviceProvider = services.BuildServiceProvider();
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
                    this.serviceProvider.GetService<IMapper>());

            this.products = new List<Product>();

            this.productRepoMock
                .Setup(x => x.FindOrDefaultAsync(It.IsAny<Expression<Func<Product, bool>>>()))
                .Returns<Expression<Func<Product, bool>>>(async x => await Task.FromResult(this.products.FirstOrDefault(x.Compile())));

            this.productRepoMock
                .Setup(x => x.GetProductWithCategoryById(It.IsAny<string>()))
                .Returns<string>(async x => await Task.FromResult(this.products.First(a => a.Id == x)));
        }

        private void SeedTestProducts()
        {
            this.products.Add(new()
            {
                Id = "4da54227-ccf1-4fd5-9fb5-21ae4356da33",
                Name = "Bananas",
                ImageUrl = "https://images.immediate.co.uk/production/volatile/sites/30/2017/01/Bananas-218094b-scaled.jpg",
                CategoryId = 1,
                Category = new ProductCategory()
                {
                    Id = 1,
                    Title = "Fruits",
                },
                Price = 12.20M,
                Description = "Test bananas",
                Quantity = 120
            });

            this.products.Add(new()
            {
                Id = "999cae77-6db9-4437-bb4f-440bcfcc8772",
                Name = "Apples",
                ImageUrl = "https://images.heb.com/is/image/HEBGrocery/000466634",
                CategoryId = 2,
                Category = new ProductCategory()
                {
                    Id = 2,
                    Title = "Vegetables",
                },
                Price = 9.20M,
                Description = "Test apples",
                Quantity = 12
            });

            this.products.Add(new()
            {
                Id = "22164c80-b0de-4633-ada5-a74ac0674843",
                Name = "Oranges",
                ImageUrl = "https://produits.bienmanger.com/33285-0w470h470_Organic_Fresh_Oranges_From_Italy_New_Hall.jpg",
                CategoryId = 3,
                Category = new ProductCategory()
                {
                    Id = 3,
                    Title = "Meat",
                },
                Price = 12,
                Description = "Test oranges",
                Quantity = 0
            });
        }

        [Test]
        [TestCase("Bananas", "https://images.immediate.co.uk/production/volatile/sites/30/2017/01/Bananas-218094b-scaled.jpg", 1, 10.10, "Test bananas", 120)]
        [TestCase("Apples", "https://images.heb.com/is/image/HEBGrocery/000466634", 2, 11, "Test apples", 300)]
        [TestCase("Oranges", "https://produits.bienmanger.com/33285-0w470h470_Organic_Fresh_Oranges_From_Italy_New_Hall.jpg", 5, 1.20, "Test oranges", 1)]
        [TestCase("WaterMelon", "https://i2.wp.com/evs-translations.com/blog/wp-content/uploads/2016/07/EVS_Translations_watermelon-pix.jpg", 10, 12.02, "Test watermelon", 0)]
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
                .Add(
                    name,
                    imageUrl,
                    categoryId,
                    price,
                    description,
                    quantity);

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
            SeedTestProducts();

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
            SeedTestProducts();

            var productFromService = await this.productService
                .GetByIdOrDefault(id);

            Assert.IsNull(productFromService);
        }
    }
}
