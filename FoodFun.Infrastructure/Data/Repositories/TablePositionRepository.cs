namespace FoodFun.Infrastructure.Data.Repositories
{
    using FoodFun.Infrastructure.Common.Contracts;
    using FoodFun.Infrastructure.Models;

    public class TablePositionRepository : EfRepository<TablePosition>, ITablePositionRepository
    {
        public TablePositionRepository(FoodFunDbContext dbContext) 
            : base(dbContext)
        {
        }
    }
}
