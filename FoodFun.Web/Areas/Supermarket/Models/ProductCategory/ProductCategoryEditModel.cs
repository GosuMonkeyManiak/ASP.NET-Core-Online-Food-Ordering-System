namespace FoodFun.Web.Areas.Supermarket.Models.ProductCategory
{
    using System.ComponentModel.DataAnnotations;
    using Core.ValidationAttributes;
    using static Infrastructure.Common.DataConstants.ProductCategory;
    using static Constants.GlobalConstants.Messages;

    public class ProductCategoryEditModel
    {
        public int Id { get; init; }

        [Required]
        [StringLength(
            TitleMaxLength,
            MinimumLength = TitleMinLength,
            ErrorMessage = CategoryTitleError)]
        [ShouldBeUniqueProductCategory]
        public string Title { get; init; }
    }
}
