namespace FoodFun.Infrastructure.Common.Contracts
{
    using Base;
    using Models;

    public interface IDishRepository : IRepository<Dish>, IBaseItemRepository
    {
        Task<IEnumerable<Dish>> All(string[] ids);

        Task<Dish> GetDishWithCategoryById(string id);

        Task<IEnumerable<Dish>> GetDishesWithCategories();

        Task<IEnumerable<Dish>> GetAllDishesWithCategories(
            string searchTerm,
            int categoryFilterId,
            byte orderNumber,
            int pageNumber,
            int pageSize,
            bool onlyAvailable);
    }
}
