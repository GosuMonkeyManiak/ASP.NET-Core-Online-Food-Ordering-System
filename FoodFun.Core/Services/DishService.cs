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
        private readonly IDishCategoryService dishCategoryService;
        private readonly IDishRepository dishRepository;
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

        public async Task Add(
            string name,
            string imageUrl,
            int categoryId,
            decimal price,
            string description,
            ulong quantity)
        {
            var dish = new Dish()
            {
                Name = name,
                ImageUrl = imageUrl,
                CategoryId = categoryId,
                Price = price,
                Description = description,
                Quantity = quantity
            };

            await this.dishRepository
                .AddAsync(dish);

            await this.dishRepository
                .SaveChangesAsync();
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

        public async Task<bool> IsDishExist(string id)
            => await this.dishRepository
                .FindOrDefaultAsync(p => p.Id == id) != null;

        public async Task<bool> Update(
            string id, 
            string name, 
            string imageUrl, 
            int categoryId, 
            decimal price, 
            string description,
            ulong quantity)
        {
            var dish = await this.dishRepository
                .GetDishWithCategoryById(id);

            var category = await this.dishCategoryService
                .GetByIdOrDefault(categoryId);

            if (dish.Category.Title != category.Title
                || dish.Name != name)
            {
                if (await this.dishCategoryService.IsItemExistInCategory(categoryId, name))
                {
                    return false;
                }
            }

            var updateDish = new Dish()
            {
                Id = id,
                Name = name,
                ImageUrl = imageUrl,
                CategoryId = categoryId,
                Price = price,
                Description = description,
                Quantity = quantity
            };

            this.dishRepository
                .Update(updateDish);

            await this.dishRepository.SaveChangesAsync();

            return true;
        }
    }
}
