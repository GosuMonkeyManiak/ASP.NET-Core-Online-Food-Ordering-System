namespace FoodFun.Web.Models.Dish
{
    public class DishListingModel
    {
        public string Id { get; init; }

        public string Name { get; set; }

        public string ImageUrl { get; init; }

        public string CategoryTitle { get; init; }

        public decimal Price { get; init; }

        public string Description { get; init; }
    }
}
