namespace FoodFun.Core.Services
{
    using Contracts;
    using Extensions;
    using global::AutoMapper;
    using Infrastructure.Common.Contracts;
    using Infrastructure.Models;
    using Models.Dish;

    public class DishService : IDishService
    {
        private readonly IDishRepository dishRepository;
        private readonly IDishCategoryService dishCategoryService;
        private readonly IMapper mapper;

        public DishService(
            IDishRepository dishRepository, 
            IDishCategoryService dishCategoryService,
            IMapper mapper)
        {
            this.dishRepository = dishRepository;
            this.dishCategoryService = dishCategoryService;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<DishServiceModel>> All()
        {
            var dishesWithCategories = await this.dishRepository.GetDishesWithCategories();

            return dishesWithCategories.ProjectTo<DishServiceModel>(this.mapper);
        }

        public async Task<DishServiceModel> GetByIdOrDefault(string id)
        {
            if (!await IsDishExist(id))
            {
                return null;
            }

            var dishWithCategory = await this.dishRepository
                .GetDishWithCategoryById(id);

            return this.mapper.Map<DishServiceModel>(dishWithCategory);
        }

        public async Task<bool> Update(
            string id, 
            string name, 
            string imageUrl, 
            int categoryId, 
            decimal price, 
            string description)
        {
            if (!await this.IsDishExist(id) ||
                !await this.dishCategoryService.IsCategoryExist(categoryId))
            {
                return false;
            }

            var dish = new Dish()
            {
                Id = id,
                Name = name,
                ImageUrl = imageUrl,
                CategoryId = categoryId,
                Price = price,
                Description = description
            };

            this.dishRepository
                .Update(dish);

            await this.dishRepository.SaveChangesAsync();

            return true;
        }

        public async Task<Tuple<bool, bool>> Add(
            string name, 
            string imageUrl, 
            int categoryId, 
            decimal price, 
            string description)
        {
            if (!await this.dishCategoryService.IsCategoryExist(categoryId))
            {
                return new(false, false);
            }

            if (await IsDishInCategory(name, categoryId))
            {
                return new(true, true);
            }

            var dish = new Dish()
            {
                Name = name,
                ImageUrl = imageUrl,
                CategoryId = categoryId,
                Price = price,
                Description = description
            };

            await this.dishRepository
                .AddAsync(dish);

            await this.dishRepository
                .SaveChangesAsync();

            return new(true, false);
        }

        private async Task<bool> IsDishExist(string id)
            => await this.dishRepository
                .FindOrDefaultAsync(p => p.Id == id) != null;

        private async Task<bool> IsDishInCategory(
            string name,
            int id)
            => await this.dishRepository
                .FindOrDefaultAsync(x => x.Name == name
                                         && x.CategoryId == id) != null;
    }
}
