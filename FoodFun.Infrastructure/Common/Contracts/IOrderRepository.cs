namespace FoodFun.Infrastructure.Common.Contracts
{
    using FoodFun.Infrastructure.Models;

    public interface IOrderRepository : IRepository<Order>
    {
        Task<IEnumerable<Order>> AllWithUsers();
    }
}
