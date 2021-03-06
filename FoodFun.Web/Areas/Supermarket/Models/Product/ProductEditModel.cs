namespace FoodFun.Web.Areas.Supermarket.Models.Product
{
    using System.ComponentModel.DataAnnotations;
    using Core.ValidationAttributes.Product;
    using Core.ValidationAttributes.ProductCategory;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Web.Models.Product;

    using static Constants.GlobalConstants;
    using static Constants.GlobalConstants.Messages;
    using static Infrastructure.Common.DataConstants;
    using static Infrastructure.Common.DataConstants.Product;

    public class ProductEditModel
    {
        [Required]
        [StringLength(
            DefaultIdLength, 
            MinimumLength = DefaultIdLength)]
        [MustBeExistingProduct]
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
        
        [MustBeExistingProductCategory]
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

        [Range(
            QuantityMinLength,
            long.MaxValue,
            ErrorMessage = QuantityError)]
        public ulong Quantity { get; init; }

        [BindNever]
        public IEnumerable<ProductCategoryModel> Categories { get; set; } = new List<ProductCategoryModel>();
    }
}
