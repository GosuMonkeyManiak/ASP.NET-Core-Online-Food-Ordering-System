namespace FoodFun.Infrastructure.Data.Repositories
{
    using FoodFun.Infrastructure.Common.Contracts;
    using FoodFun.Infrastructure.Models;

    public class TableSizeRepository : EfRepository<TableSize>, ITableSizeRepository
    {
        public TableSizeRepository(FoodFunDbContext dbContext) 
            : base(dbContext)
        {
        }
    }
}
