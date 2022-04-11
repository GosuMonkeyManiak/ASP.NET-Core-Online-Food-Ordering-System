namespace FoodFun.Infrastructure.Data.Repositories
{
    using FoodFun.Infrastructure.Common.Contracts;
    using FoodFun.Infrastructure.Models;

    public class TableRepository : EfRepository<Table>, ITableRepository
    {
        public TableRepository(FoodFunDbContext dbContext) 
            : base(dbContext)
        {
        }
    }
}
