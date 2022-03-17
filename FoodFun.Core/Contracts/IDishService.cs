namespace FoodFun.Core.Contracts
{
    public interface IDishService
    {
        Task<bool> Add(
            string name,
            string imageUrl,
            int categoryId,
            decimal price,
            string description);
    }
}
