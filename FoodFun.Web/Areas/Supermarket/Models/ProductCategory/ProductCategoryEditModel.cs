namespace FoodFun.Web.Areas.Supermarket.Models.ProductCategory
{
    using System.ComponentModel.DataAnnotations;
    using Core.ValidationAttributes.Product;
    using Core.ValidationAttributes.ProductCategory;

    using static Constants.GlobalConstants;
    using static Infrastructure.Common.DataConstants.ProductCategory;
    using static Constants.GlobalConstants.Messages;

    public class ProductCategoryEditModel
    {
        [Required]
        [StringLength(
            DefaultIdLength,
            MinimumLength = DefaultIdLength)]
        [MustBeExistingProductCategory]
        public int Id { get; init; }

        [Required]
        [StringLength(
            TitleMaxLength,
            MinimumLength = TitleMinLength,
            ErrorMessage = CategoryTitleError)]
        [MustBeUniqueProductCategoryWithTitle]
        public string Title { get; init; }
    }
}
