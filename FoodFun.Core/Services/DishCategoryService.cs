namespace FoodFun.Core.Services
{
    using Contracts;
    using Extensions;
    using global::AutoMapper;
    using Infrastructure.Common.Contracts;
    using Infrastructure.Data;
    using Infrastructure.Models;
    using Microsoft.EntityFrameworkCore;
    using Models.DishCategory;

    public class DishCategoryService : IDishCategoryService
    {
        private readonly IDishCategoryRepository dishCategoryRepository;
        private readonly IMapper mapper;

        public DishCategoryService(
            IDishCategoryRepository dishCategoryRepository,
            IMapper mapper)
        {
            this.dishCategoryRepository = dishCategoryRepository;
            this.mapper = mapper;
        }

        public async Task<bool> IsCategoryExist(int id)
            => await this.dishCategoryRepository
                .FindOrDefaultAsync(x => x.Id == id) != null;

        public async Task<IEnumerable<DishCategoryServiceModel>> All()
        {
            var categories = await this.dishCategoryRepository
                .AllAsNoTracking();

            return categories.ProjectTo<DishCategoryServiceModel>(this.mapper);
        }

        public async Task<bool> Add(string title)
        {
            var isCategoryExist = this.dishCategoryRepository
                .FindOrDefaultAsync(x => x.Title == title) != null;

            if (isCategoryExist)
            {
                return false;
            }

            var dishCategory = new DishCategory()
            {
                Title = title
            };

            await this.dishCategoryRepository
                .AddAsync(dishCategory);

            await this.dishCategoryRepository
                .SaveChangesAsync();

            return true;
        }
    }
}
