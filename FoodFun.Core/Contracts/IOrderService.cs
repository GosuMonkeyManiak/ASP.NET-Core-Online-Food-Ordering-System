namespace FoodFun.Core.Contracts
{
    using FoodFun.Core.Models.Cart;

    public interface IOrderService
    {
        Task<int> Create(
            string userId,
            IEnumerable<CartItemModel> products,
            IEnumerable<CartItemModel> dishes);
    }
}
