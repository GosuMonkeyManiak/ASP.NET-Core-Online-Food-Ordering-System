namespace FoodFun.Web.Areas.Order.Models.Order
{
    public class OrderWithItemsListingModel
    {
        public string UserEmail { get; init; }

        public decimal Price { get; init; }

        public IEnumerable<OrderItemModel> Products { get; init; }

        public IEnumerable<OrderItemModel> Dishes { get; init; }
    }
}
