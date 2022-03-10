namespace FoodFun.Web.Models.ProductCategory
{
    using System.ComponentModel.DataAnnotations;

    using static Constants.GlobalConstants.Messages;
    using static Infrastructure.Data.Common.DataConstants.ProductCategory;

    public class ProductCategoryFormModel
    {
        [Required]
        [StringLength(
            TitleMaxLength,
            MinimumLength = TitleMinLength,
            ErrorMessage = ProductCategoryTitleError)]
        public string Title { get; init; }
    }
}
