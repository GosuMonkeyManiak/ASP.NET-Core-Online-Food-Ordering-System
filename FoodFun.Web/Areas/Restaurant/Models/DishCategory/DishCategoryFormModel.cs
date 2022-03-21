namespace FoodFun.Web.Areas.Restaurant.Models.DishCategory
{
    using System.ComponentModel.DataAnnotations;

    using static Infrastructure.Common.DataConstants.DishCategory;
    using static Constants.GlobalConstants.Messages;

    public class DishCategoryFormModel
    {
        [Required]
        [StringLength(
            TitleMaxLength,
            MinimumLength = TitleMinLength,
            ErrorMessage = CategoryTitleError)]
        public string Title { get; init; }
    }
}
