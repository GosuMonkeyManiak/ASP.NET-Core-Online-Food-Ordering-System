namespace FoodFun.Infrastructure.Data.Repositories
{
    using Common.Contracts;
    using Models;

    public class OrderRepository : EfRepository<Order>, IOrderRepository
    {
        public OrderRepository(FoodFunDbContext dbContext) 
            : base(dbContext)
        {
        }
    }
}
