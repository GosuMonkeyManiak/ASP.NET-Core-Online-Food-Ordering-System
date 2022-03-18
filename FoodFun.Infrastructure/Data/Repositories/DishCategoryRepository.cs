namespace FoodFun.Infrastructure.Data.Repositories
{
    using Common.Contracts;
    using Models;

    public class DishCategoryRepository : EfRepository<DishCategory>, IDishCategoryRepository
    {
        public DishCategoryRepository(FoodFunDbContext dbContext) 
            : base(dbContext)
        {
        }
    }
}
