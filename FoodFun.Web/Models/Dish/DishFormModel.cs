namespace FoodFun.Web.Models.Dish
{
    using System.ComponentModel.DataAnnotations;
    using DishCategory;
    using Microsoft.AspNetCore.Mvc.ModelBinding;

    using static Infrastructure.Data.Common.DataConstants;
    using static Infrastructure.Data.Common.DataConstants.Dish;
    using static Constants.GlobalConstants.Messages;

    public class DishFormModel
    {
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

        [BindNever]
        public IEnumerable<DishCategoryModel> Categories { get; init; } = new List<DishCategoryModel>();
    }
}
