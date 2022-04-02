namespace FoodFun.Web.Areas.Restaurant.Models.DishCategory
{
    using System.ComponentModel.DataAnnotations;
    using Core.ValidationAttributes.DishCategory;

    using static Constants.GlobalConstants.Messages;
    using static Infrastructure.Common.DataConstants.DishCategory;

    public class DishCategoryEditModel
    {
        [MustBeExistingDishCategory]
        public int Id { get; init; }

        [Required]
        [StringLength(
            TitleMaxLength,
            MinimumLength = TitleMinLength,
            ErrorMessage = CategoryTitleError)]
        [MustBeUniqueDishCategoryTitle]
        public string Title { get; init; }
    }
}
