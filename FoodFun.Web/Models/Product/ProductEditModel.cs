namespace FoodFun.Web.Models.Product
{
    using System.ComponentModel.DataAnnotations;
    using Core.Models.ProductCategory;
    using Microsoft.AspNetCore.Mvc.ModelBinding;

    using static Constants.GlobalConstants;
    using static Constants.GlobalConstants.Messages;
    using static Infrastructure.Data.Common.DataConstants;
    using static Infrastructure.Data.Common.DataConstants.Product;

    public class ProductEditModel
    {
        [Required]
        [StringLength(
            DefaultIdLength, 
            MinimumLength = DefaultIdLength)]
        public string Id { get; init; }

        [Required]
        [StringLength(
            NameMaxLength,
            MinimumLength = NameMinLength,
            ErrorMessage = ProductNameError)]
        public string Name { get; init; }

        [Required]
        [Url(ErrorMessage = UrlError)]
        [MaxLength(
            UrlMaxLength,
            ErrorMessage = UrlErrorWithMaxLength)]
        [Display(Name = "Image Url")]
        public string ImageUrl { get; init; }

        [Display(Name = "Category")]
        public int CategoryId { get; init; }

        [Range(
            typeof(decimal),
            PriceMinLength,
            PriceMaxLength,
            ErrorMessage = ProductPriceError)]
        public decimal Price { get; init; }

        [Required]
        [MinLength(
            DescriptionMinLength,
            ErrorMessage = DescriptionError)]
        public string Description { get; init; }

        [BindNever]
        public IEnumerable<ProductCategoryWithProductCountServiceModel> Categories { get; init; } = new List<ProductCategoryWithProductCountServiceModel>();
    }
}
