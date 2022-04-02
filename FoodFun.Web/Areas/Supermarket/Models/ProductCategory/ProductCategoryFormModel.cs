namespace FoodFun.Web.Areas.Supermarket.Models.ProductCategory
{
    using System.ComponentModel.DataAnnotations;
    using Core.ValidationAttributes.ProductCategory;
    using static Constants.GlobalConstants.Messages;
    using static Infrastructure.Common.DataConstants.ProductCategory;

    public class ProductCategoryFormModel
    {
        [Required]
        [StringLength(
            TitleMaxLength,
            MinimumLength = TitleMinLength,
            ErrorMessage = CategoryTitleError)]
        [MustBeUniqueProductCategoryTitle]
        public string Title { get; init; }
    }
}
