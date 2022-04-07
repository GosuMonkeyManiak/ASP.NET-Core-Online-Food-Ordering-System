namespace FoodFun.Web.Areas.Order.Models.Order
{
    public class OrderListingModel
    {
        public int Id { get; init; }

        public string UserEmail { get; init; }

        public decimal Price { get; init; }

        public bool IsSent { get; init; }

        public bool IsDelivered { get; init; }
    }
}
