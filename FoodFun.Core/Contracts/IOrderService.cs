namespace FoodFun.Core.Contracts
{
    using FoodFun.Core.Models.Cart;
    using FoodFun.Core.Models.Order;

    public interface IOrderService
    {
        Task<int> Create(
            string userId,
            IEnumerable<CartItemModel> products,
            IEnumerable<CartItemModel> dishes);

        Task<IEnumerable<OrderServiceModel>> All(bool onlyActive = true);

        Task<OrderWithItemsServiceModel> ByIdWithItems(int id);

        Task<bool> IsOrderExist(int id);

        Task Sent(int id);
        Task Deliver(int id);
    }
}
