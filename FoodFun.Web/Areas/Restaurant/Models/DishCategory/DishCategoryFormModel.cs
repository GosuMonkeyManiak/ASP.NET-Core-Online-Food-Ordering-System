namespace FoodFun.Web.Areas.Restaurant.Models.DishCategory
{
    using System.ComponentModel.DataAnnotations;
    using Core.ValidationAttributes.DishCategory;
    using static Infrastructure.Common.DataConstants.DishCategory;
    using static Constants.GlobalConstants.Messages;

    public class DishCategoryFormModel
    {
        [Required]
        [StringLength(
            TitleMaxLength,
            MinimumLength = TitleMinLength,
            ErrorMessage = CategoryTitleError)]
        [MustBeUniqueDishCategoryTitle]
        public string Title { get; init; }
    }
}
