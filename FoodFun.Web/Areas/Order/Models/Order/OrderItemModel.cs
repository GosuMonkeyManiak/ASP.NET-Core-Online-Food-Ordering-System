namespace FoodFun.Web.Areas.Order.Models.Order
{
    public class OrderItemModel
    {
        public string Name { get; init; }

        public decimal Price { get; init; }

        public ulong Quantity { get; init; }
    }
}
