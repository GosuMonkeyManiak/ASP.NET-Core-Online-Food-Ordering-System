namespace FoodFun.Web.Areas.Supermarket.Models.ProductCategory
{
    using System.ComponentModel.DataAnnotations;
    using Core.ValidationAttributes;
    using static Constants.GlobalConstants.Messages;
    using static Infrastructure.Common.DataConstants.ProductCategory;

    public class ProductCategoryFormModel
    {
        [Required]
        [StringLength(
            TitleMaxLength,
            MinimumLength = TitleMinLength,
            ErrorMessage = CategoryTitleError)]
        [ShouldBeUniqueProductCategory]
        public string Title { get; init; }
    }
}
