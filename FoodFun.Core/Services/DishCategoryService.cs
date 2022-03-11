namespace FoodFun.Core.Services
{
    using Contracts;
    using Infrastructure.Data;
    using Microsoft.EntityFrameworkCore;
    using Models.DishCategory;

    public class DishCategoryService : IDishCategoryService
    {
        private readonly FoodFunDbContext dbContext;

        public DishCategoryService(FoodFunDbContext dbContext)
            => this.dbContext = dbContext;

        public async Task<bool> IsCategoryExist(int categoryId)
            => await this.dbContext
                .DishesCategories
                .AnyAsync(dc => dc.Id == categoryId);

        public async Task<IEnumerable<DishCategoryServiceModel>> All()
            => await this.dbContext
                .DishesCategories
                .Select(dc => new DishCategoryServiceModel()
                {
                    Id = dc.Id,
                    Title = dc.Title
                })
                .ToListAsync();
    }
}
