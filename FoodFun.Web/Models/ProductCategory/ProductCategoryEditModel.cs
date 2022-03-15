namespace FoodFun.Web.Models.ProductCategory
{
    using System.ComponentModel.DataAnnotations;

    using static Infrastructure.Common.DataConstants.ProductCategory;
    using static Constants.GlobalConstants.Messages;

    public class ProductCategoryEditModel
    {
        public int Id { get; init; }

        [Required]
        [StringLength(
            TitleMaxLength,
            MinimumLength = TitleMinLength,
            ErrorMessage = ProductCategoryTitleError)]
        public string Title { get; init; }
    }
}
