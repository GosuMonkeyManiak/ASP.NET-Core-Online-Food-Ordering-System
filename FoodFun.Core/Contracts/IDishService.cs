namespace FoodFun.Core.Contracts
{
    using Models.Dish;

    public interface IDishService
    {
        Task<Tuple<IEnumerable<DishServiceModel>, int, int, int>> All(
            string searchTerm,
            int categoryFilterId,
            byte orderNumber,
            int pageNumber,
            int pageSize,
            bool onlyAvailable = true);

        Task<DishServiceModel> GetByIdOrDefault(string id);

        Task<bool> Update(
            string id,
            string name,
            string imageUrl,
            int categoryId,
            decimal price,
            string description,
            ulong quantity);

        Task Add(
            string name,
            string imageUrl,
            int categoryId,
            decimal price,
            string description,
            ulong quantity);

        Task<bool> IsDishExist(string id);
    }
}
