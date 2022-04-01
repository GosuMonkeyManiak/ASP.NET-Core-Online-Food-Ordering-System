namespace FoodFun.Core.Models.Dish
{
    using DishCategory;

    public class DishServiceModel
    {
        public string Id { get; init; }

        public string Name { get; init; }

        public string ImageUrl { get; init; }

        public DishCategoryServiceModel Category { get; init; }

        public decimal Price { get; init; }

        public string Description { get; init; }

        public ulong Quantity { get; init; }
    }
}
