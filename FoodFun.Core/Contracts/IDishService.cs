namespace FoodFun.Core.Contracts
{
    using Models.Dish;

    public interface IDishService
    {
        Task<IEnumerable<DishServiceModel>> All();

        Task<DishServiceModel> GetByIdOrDefault(string id);

        Task<bool> Add(
            string name,
            string imageUrl,
            int categoryId,
            decimal price,
            string description);
    }
}
