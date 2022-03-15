namespace FoodFun.Web.Models.Product
{
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using ProductCategory;
    using System.ComponentModel.DataAnnotations;

    using static Constants.GlobalConstants.Messages;
    using static Infrastructure.Common.DataConstants;
    using static Infrastructure.Common.DataConstants.Product;

    public class ProductFormModel
    {
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
        public IEnumerable<ProductCategoryEditModel> Categories { get; init; } = new List<ProductCategoryEditModel>();
    }
}
