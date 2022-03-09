﻿namespace FoodFun.Web.Areas.Products.Models
{
    using System.ComponentModel.DataAnnotations;
    using Core.Models.Products;
    using Microsoft.AspNetCore.Mvc.ModelBinding;

    using static Infrastructure.Data.Common.DataConstants.Product;
    using static Infrastructure.Data.Common.DataConstants;

    using static Constants.GlobalConstants.Messages;

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
        public IEnumerable<ProductCategoryServiceModel> Categories { get; init; } = new List<ProductCategoryServiceModel>();
    }
}
