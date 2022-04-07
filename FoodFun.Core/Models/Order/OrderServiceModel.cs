namespace FoodFun.Core.Models.Order
{
    public class OrderServiceModel
    {
        public int Id { get; init; }

        public string UserEmail { get; init; }

        public string Price { get; init; }

        public bool IsSent { get; init; }

        public bool IsDelivered { get; init; }
    }
}
