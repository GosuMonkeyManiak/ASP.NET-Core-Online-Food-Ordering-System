namespace FoodFun.Web.Models.DishCategory
{
    using System.ComponentModel.DataAnnotations;

    using static Constants.GlobalConstants.Messages;
    using static Infrastructure.Common.DataConstants.DishCategory;

    public class DishCategoryEditModel
    {
        public int Id { get; init; }

        [Required]
        [StringLength(
            TitleMaxLength,
            MinimumLength = TitleMinLength,
            ErrorMessage = CategoryTitleError)]
        public string Title { get; init; }
    }
}
