namespace FoodFun.Infrastructure.Data.Repositories
{
    using Common.Contracts;
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class DishCategoryRepository : EfRepository<DishCategory>, IDishCategoryRepository
    {
        public DishCategoryRepository(FoodFunDbContext dbContext) 
            : base(dbContext)
        {
        }

        public async Task<IEnumerable<DishCategory>> GetAllWithDishes()
            => await this.DbSet
                .Include(x => x.Dishes)
                .AsNoTracking()
                .ToListAsync();
    }
}
