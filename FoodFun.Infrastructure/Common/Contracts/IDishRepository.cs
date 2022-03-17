namespace FoodFun.Infrastructure.Common.Contracts
{
    using Models;

    public interface IDishRepository : IRepository<Dish>
    {
        Task<Dish> GetDishWithCategoryById(string id);

        Task<IEnumerable<Dish>> GetDishesWithCategories();
    }
}
