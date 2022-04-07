namespace FoodFun.Core.Models.Order
{
    using FoodFun.Core.Models.Dish;
    using Product;
    using System.Collections.Generic;

    public class OrderWithItemsServiceModel
    {
        public string UserEmail { get; init; }

        public decimal Price { get; init; }

        public IEnumerable<ProductServiceModel> Products { get; init; }

        public IEnumerable<DishServiceModel> Dishes { get; init; }
    }
}
