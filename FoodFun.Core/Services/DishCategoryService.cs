namespace FoodFun.Core.Services
{
    using Contracts;
    using Extensions;
    using global::AutoMapper;
    using Infrastructure.Common.Contracts;
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

        public async Task Add(string title)
        {
            await this.dishCategoryRepository
                .AddAsync(new() { Title = title });

            await this.dishCategoryRepository
                .SaveChangesAsync();
        }

        public async Task<IEnumerable<DishCategoryServiceModel>> All()
        {
            var categories = await this.dishCategoryRepository
                .AllAsNoTracking();

            return categories.ProjectTo<DishCategoryServiceModel>(this.mapper);
        }

        public async Task<IEnumerable<DishCategoryServiceModel>> AllNotDisabled()
            => (await this.dishCategoryRepository.GeAllNotDisabled())
                .ProjectTo<DishCategoryServiceModel>(this.mapper);

        public async Task<IEnumerable<DishCategoryWithDishCountServiceModel>> AllWithDishesCount()
        {
            var categoriesWithDishes = await this.dishCategoryRepository
                .GetAllWithDishes();

            return categoriesWithDishes.ProjectTo<DishCategoryWithDishCountServiceModel>(this.mapper);
        }

        public async Task Disable(int id)
        {
            var category = await this.dishCategoryRepository
                .FindAsync(x => x.Id == id);

            category.IsDisable = true;

            this.dishCategoryRepository
                .Update(category);

            await this.dishCategoryRepository
                .SaveChangesAsync();
        }

        public async Task Enable(int id)
        {
            var category = await this.dishCategoryRepository
                .FindAsync(x => x.Id == id);

            category.IsDisable = false;

            this.dishCategoryRepository
                .Update(category);

            await this.dishCategoryRepository
                .SaveChangesAsync();
        }

        public async Task<DishCategoryServiceModel> GetByIdOrDefault(int id)
        {
            if (!await IsCategoryExist(id))
            {
                return null;
            }

            var category = await this.dishCategoryRepository
                .FindOrDefaultAsync(x => x.Id == id);

            return this.mapper.Map<DishCategoryServiceModel>(category);
        }

        public async Task<bool> IsCategoryActive(int id)
            => !(await this.dishCategoryRepository
                    .FindOrDefaultAsync(x => x.Id == id))
                .IsDisable;

        public async Task<bool> IsCategoryExist(int id)
            => await this.dishCategoryRepository
                .FindOrDefaultAsync(x => x.Id == id) != null;

        public async Task<bool> IsCategoryExist(string title)
            => await this.dishCategoryRepository
                .FindOrDefaultAsync(x => x.Title == title) != null;

        public async Task<bool> IsItemExistInCategory(int categoryId, string itemName)
            => (await this.dishCategoryRepository
                    .FindOrDefaultAsync(x => x.Id == categoryId))
                .Dishes
                .Any(x => x.Name == itemName);

        public async Task<bool> IsItemInAnyActiveCategory(string itemId)
            => (await this.dishCategoryRepository
                    .GetAllWithDishes())
                .Any(x => !x.IsDisable && x.Dishes.FirstOrDefault(d => d.Id == itemId) != null);

        public async Task Update(int id, string title)
        {
            this.dishCategoryRepository
                .Update(new() { Id = id, Title = title});

            await this.dishCategoryRepository
                .SaveChangesAsync();
        }
    }
}
