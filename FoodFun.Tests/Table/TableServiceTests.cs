namespace FoodFun.Tests.Table
{
    using AutoMapper;
    using FoodFun.Core.AutoMapper;
    using FoodFun.Core.Contracts;
    using FoodFun.Core.Services;
    using FoodFun.Infrastructure.Common.Contracts;
    using FoodFun.Infrastructure.Models;
    using Microsoft.Extensions.DependencyInjection;
    using Moq;
    using NUnit.Framework;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Threading.Tasks;
    using System;

    [TestFixture]
    public class TableServiceTests
    {
        private readonly ITableService tableService;

        private Mock<ITableRepository> tableRepositoryMock;
        private IMapper mapper;

        private IList<Table> tables;

        public TableServiceTests()
        {
            var serviceCollection = new ServiceCollection()
                    .AddAutoMapper(typeof(AutoMapperServiceProfile));
            var serviceProvider = serviceCollection.BuildServiceProvider();

            this.tableRepositoryMock = new Mock<ITableRepository>();
            this.mapper = serviceProvider.GetService<IMapper>();

            this.tableService = new TableService(
                this.tableRepositoryMock.Object,
                this.mapper);
        }

        [SetUp]
        public void SetUp()
        {
            this.tables = new List<Table>();

            MockRepoMethods();
        }

        private void MockRepoMethods()
        {
            this.tableRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<Table>()))
                .Callback<Table>(x => this.tables.Add(x));

            this.tableRepositoryMock
                .Setup(x => x.FindOrDefaultAsync(It.IsAny<Expression<Func<Table, bool>>>()))
                .Returns<Expression<Func<Table, bool>>>(async x => 
                    await Task.FromResult(this.tables.FirstOrDefault(x.Compile())));

            this.tableRepositoryMock
                .Setup(x => x.GetWithPositionAndSizeById(It.IsAny<string>()))
                .Returns<string>(async x => await Task.FromResult(this.tables.First(a => a.Id == x)));

            this.tableRepositoryMock
                .Setup(x => x.Update(It.IsAny<Table>()))
                .Callback<Table>(x =>
                {
                    var table = this.tables.First(a => a.Id == x.Id);
                    table.TableSizeId = x.TableSizeId;
                    table.TablePositionId = x.TablePositionId;
                });

            this.tableRepositoryMock
                .Setup(x => x.AllWithPositionsAndSizes(It.IsAny<string>()))
                .ReturnsAsync(this.tables.AsEnumerable());
        }

        private void SeedTestTables()
        {
            var inside = new TablePosition() { Id = 1, Position = "Inside" };
            var outSide = new TablePosition() { Id = 2, Position = "Outside" };

            var sizeForTwo = new TableSize() { Id = 2, Seats = 2 };
            var sizeForThere = new TableSize() { Id = 3, Seats = 3 };
            var sizeForFour = new TableSize() { Id = 4, Seats = 4 };

            this.tables.Add(new()
            {
                Id = "cbdffb71-7ef7-4ce6-b098-4f95ad8bd8d2",
                TablePositionId = 1,
                TablePosition = inside,
                TableSizeId = 2,
                TableSize = sizeForTwo,
            });
            this.tables.Add(new()
            {
                Id = "1768c182-bd51-4d2b-a3df-c3e4acfa3a1f",
                TablePositionId = 2,
                TablePosition = outSide,
                TableSizeId = 4,
                TableSize = sizeForFour,
            });
            this.tables.Add(new()
            {
                Id = "36c37479-5da5-42c5-8101-b3e849f64508",
                TablePositionId = 1,
                TablePosition = inside,
                TableSizeId = 3,
                TableSize = sizeForThere
            });
            this.tables.Add(new()
            {
                Id = "6be1898b-c434-4602-8f4e-07fdf575b1ed",
                TablePositionId = 2,
                TablePosition = outSide,
                TableSizeId = 2,
                TableSize = sizeForTwo
            });
            this.tables.Add(new()
            {
                Id = "70684b4a-7673-46ba-a9bc-a9996645a56f",
                TablePositionId = 1,
                TablePosition = inside,
                TableSizeId = 3,
                TableSize = sizeForThere
            });
            this.tables.Add(new()
            {
                Id = "e8c6713b-9d65-4c04-9717-9e3131ad8087",
                TablePositionId = 2,
                TablePosition = outSide,
                TableSizeId = 3,
                TableSize = sizeForThere
            });
        }

        [Test]
        public void When_Create_New_Instance_Should_Set_Field()
        {
            var repositoryFieldValue = this.tableService
                .GetType()
                .GetField("tableRepository", BindingFlags.Instance | BindingFlags.NonPublic)
                .GetValue(this.tableService);

            var mapperFieldValue = this.tableService
                .GetType()
                .GetField("mapper", BindingFlags.Instance | BindingFlags.NonPublic)
                .GetValue(this.tableService);

            Assert.AreEqual(this.tableRepositoryMock.Object, repositoryFieldValue);
            Assert.AreEqual(this.mapper, mapperFieldValue);
        }

        [Test]
        [TestCase(1, 2)]
        [TestCase(2, 1)]
        [TestCase(4, 5)]
        public async Task When_Call_Add_Should_AddNewTableToBackingStore(int sizeId, int positionId)
        {
            await this.tableService.Add(sizeId, positionId);

            var actualResult = this.tables.First();

            Assert.AreEqual(sizeId, actualResult.TableSizeId);
            Assert.AreEqual(positionId, actualResult.TablePositionId);
        }

        [Test]
        [TestCase("cbdffb71-7ef7-4ce6-b098-4f95ad8bd8d2")]
        [TestCase("36c37479-5da5-42c5-8101-b3e849f64508")]
        [TestCase("6be1898b-c434-4602-8f4e-07fdf575b1ed")]
        public async Task When_Call_ITableExist_WithValidId_Should_Return_True(string id)
        {
            SeedTestTables();

            var actualResult = await this.tableService.IsTableExist(id);

            Assert.IsTrue(actualResult);
        }

        [Test]
        [TestCase("cbdffb71-7ef7-4ce6-b098-4f95ad8bd8d1")]
        [TestCase("36c37479-5da5-42c5-8101-b3e849f6450a")]
        [TestCase("6be1898b-c434-4602-8f4e-07fdf575b1e2")]
        public async Task When_Call_ITableExist_WithInValidId_Should_Return_False(string id)
        {
            SeedTestTables();

            var actualResult = await this.tableService.IsTableExist(id);

            Assert.IsFalse(actualResult);
        }

        [Test]
        [TestCase("cbdffb71-7ef7-4ce6-b098-4f95ad8bd8d2")]
        [TestCase("36c37479-5da5-42c5-8101-b3e849f64508")]
        [TestCase("6be1898b-c434-4602-8f4e-07fdf575b1ed")]
        public async Task When_Call_GetByIdOrDefault_With_ValidId_Should_Return_Table(string id)
        {
            SeedTestTables();

            var actualResult = await this.tableService.GetByIdOrDefault(id);

            Assert.IsNotNull(actualResult);
        }

        [Test]
        [TestCase("cbdffb71-7ef7-4ce6-b098-4f95ad8bd8d1")]
        [TestCase("36c37479-5da5-42c5-8101-b3e849f6450a")]
        [TestCase("6be1898b-c434-4602-8f4e-07fdf575b1e2")]
        public async Task When_Call_GetByIdOrDefault_With_InValidId_Should_Return_Null(string id)
        {
            SeedTestTables();

            var actualResult = await this.tableService.GetByIdOrDefault(id);

            Assert.IsNull(actualResult);
        }

        [Test]
        [TestCase("cbdffb71-7ef7-4ce6-b098-4f95ad8bd8d2", 10, 2)]
        [TestCase("36c37479-5da5-42c5-8101-b3e849f64508", 3, 1)]
        [TestCase("6be1898b-c434-4602-8f4e-07fdf575b1ed", 4, 2)]
        public async Task When_Call_Update_Should_Update_Entity_In_Backing_Store(
            string id,
            int positionId,
            int sizeId)
        {
            SeedTestTables();

            await this.tableService.Update(id, positionId, sizeId);

            var actualResult = this.tables.First(x => x.Id == id);

            Assert.AreEqual(positionId, actualResult.TablePositionId);
            Assert.AreEqual(sizeId, actualResult.TableSizeId);
        }

        [Test]
        public async Task When_Call_All_Should_Return_All_Tables()
        {
            SeedTestTables();

            var actualResult = (await this.tableService.All()).ToList();

            Assert.AreEqual(this.tables.Count, actualResult.Count);

            for (int i = 0; i < this.tables.Count; i++)
            {
                Assert.AreEqual(this.tables[i].Id, actualResult[i].Id);
                Assert.AreEqual(this.tables[i].TablePosition.Position, actualResult[i].TablePosition);
                Assert.AreEqual(this.tables[i].TableSize.Seats, actualResult[i].TableSize);
            }
        }
    }
}
