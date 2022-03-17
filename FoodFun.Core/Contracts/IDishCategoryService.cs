namespace FoodFun.Core.Contracts
{
    using Models.DishCategory;

    public interface IDishCategoryService
    {
        Task<bool> IsCategoryExist(int Id);

        Task<IEnumerable<DishCategoryServiceModel>> All();
    }
}
