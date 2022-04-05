namespace FoodFun.Infrastructure.Common.Contracts
{
    using Models;

    public interface IDishCategoryRepository : IRepository<DishCategory>
    {
        Task<IEnumerable<DishCategory>> GetAllWithDishes();

        Task<IEnumerable<DishCategory>> GeAllNotDisabled();
    }
}
