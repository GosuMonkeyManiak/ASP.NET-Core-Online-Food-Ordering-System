namespace FoodFun.Infrastructure.Data.Repositories
{
    using Models;

    public class DishRepository : EfRepository<Dish>
    {
        public DishRepository(FoodFunDbContext dbContext) 
            : base(dbContext)
        {
        }
    }
}
