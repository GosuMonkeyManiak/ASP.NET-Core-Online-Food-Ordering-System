namespace FoodFun.Web.Areas.Restaurant.Models.Dish
{
    using System.ComponentModel.DataAnnotations;
    using Core.ValidationAttributes.Dish;
    using Core.ValidationAttributes.DishCategory;
    using DishCategory;
    using Microsoft.AspNetCore.Mvc.ModelBinding;

    using static Constants.GlobalConstants.Messages;
    using static Infrastructure.Common.DataConstants;
    using static Infrastructure.Common.DataConstants.Dish;

    public class DishEditModel
    {
        [MustBeExistingDish]
        public string Id { get; init; }

        [Required]
        [StringLength(
            NameMaxLength,
            MinimumLength = NameMinLength,
            ErrorMessage = DishNameError)]
        public string Name { get; init; }

        [Required]
        [Url(ErrorMessage = UrlError)]
        [MaxLength(
            UrlMaxLength,
            ErrorMessage = UrlErrorWithMaxLength)]
        [Display(Name = "Image Url")]
        public string ImageUrl { get; init; }

        [MustBeExistingDishCategory]
        [Display(Name = "Category")]
        public int CategoryId { get; init; }

        [Range(
            typeof(decimal),
            PriceMinLength,
            PriceMaxLength,
            ErrorMessage = DishPriceError)]
        public decimal Price { get; init; }

        [Required]
        [MinLength(
            DescriptionMinLength,
            ErrorMessage = DescriptionError)]
        public string Description { get; init; }

        [Range(
            QuantityMinLength,
            long.MaxValue,
            ErrorMessage = ProductQuantityError)]
        public ulong Quantity { get; init; }

        [BindNever]
        public IEnumerable<DishCategoryModel> Categories { get; set; } = new List<DishCategoryModel>();
    }
}
