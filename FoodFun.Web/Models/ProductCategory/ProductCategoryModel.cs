﻿namespace FoodFun.Web.Models.ProductCategory
{
    using System.ComponentModel.DataAnnotations;

    using static Infrastructure.Data.Common.DataConstants.ProductCategory;
    using static Constants.GlobalConstants.Messages;

    public class ProductCategoryModel
    {
        [Range(
            0, 
            int.MaxValue,
            ErrorMessage = ProductCategoryIdError)]
        public int Id { get; init; }

        [Required]
        [StringLength(
            TitleMaxLength,
            MinimumLength = TitleMinLength,
            ErrorMessage = ProductCategoryTitleError)]
        public string Title { get; init; }
    }
}
