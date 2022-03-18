namespace FoodFun.Core.Contracts
{
    using Models.DishCategory;

    public interface IDishCategoryService
    {
        Task<bool> IsCategoryExist(int id);

        Task<IEnumerable<DishCategoryServiceModel>> All();

        Task<IEnumerable<DishCategoryWithDishCountServiceModel>> AllWithDishesCount();

        Task<DishCategoryServiceModel> GetByIdOrDefault(int id);

        Task<bool> Update(
            int id,
            string title);

        Task<bool> Add(string title);
    }
}
