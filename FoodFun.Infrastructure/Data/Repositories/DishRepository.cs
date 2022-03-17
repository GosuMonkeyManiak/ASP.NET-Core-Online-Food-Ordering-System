namespace FoodFun.Infrastructure.Data.Repositories
{
    using Common.Contracts;
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class DishRepository : EfRepository<Dish>, IDishRepository
    {
        public DishRepository(FoodFunDbContext dbContext) 
            : base(dbContext)
        {
        }

        public async Task<Dish> GetDishWithCategoryById(string id)
            => await this.DbSet
                .Include(x => x.Category)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

        public async Task<IEnumerable<Dish>> GetDishesWithCategories()
            => await this.DbSet
                .Include(x => x.Category)
                .AsNoTracking()
                .ToListAsync();
    }
}
