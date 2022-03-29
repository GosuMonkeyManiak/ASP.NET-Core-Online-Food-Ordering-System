namespace FoodFun.Core.Contracts
{
    using Models.DishCategory;

    public interface IDishCategoryService : ICategory
    {
        Task<IEnumerable<DishCategoryServiceModel>> All();

        Task<IEnumerable<DishCategoryWithDishCountServiceModel>> AllWithDishesCount();

        Task<DishCategoryServiceModel> GetByIdOrDefault(int id);

        Task Update(
            int id,
            string title);

        Task Add(string title);
    }
}
