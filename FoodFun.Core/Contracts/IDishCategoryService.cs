namespace FoodFun.Core.Contracts
{
    using Models.DishCategory;

    public interface IDishCategoryService
    {
        Task<bool> IsCategoryExist(int categoryId);

        Task<IEnumerable<DishCategoryServiceModel>> All();
    }
}
