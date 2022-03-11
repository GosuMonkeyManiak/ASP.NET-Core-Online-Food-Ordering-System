namespace FoodFun.Core.Contracts
{
    public interface IDishService
    {
        Task<Tuple<bool, IEnumerable<string>>> Add(
            string name,
            string imageUrl,
            int categoryId,
            decimal price,
            string description);
    }
}
