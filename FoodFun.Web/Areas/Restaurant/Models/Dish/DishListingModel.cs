namespace FoodFun.Web.Areas.Restaurant.Models.Dish
{
    using FoodFun.Core.ValidationAttributes.Dish;
    using System.ComponentModel.DataAnnotations;

    using static Web.Constants.GlobalConstants.Messages;

    public class DishListingModel
    {
        [Required]
        [MustBeExistingDish]
        [MustBeInActiveDishCategory]
        public string Id { get; init; }

        public string Name { get; set; }

        public string ImageUrl { get; init; }

        public string CategoryTitle { get; init; }

        public decimal Price { get; init; }

        public string Description { get; init; }

        [Range(
            1,
            ulong.MaxValue,
            ErrorMessage = QuantityError)]
        public ulong Quantity { get; set; }
    }
}
