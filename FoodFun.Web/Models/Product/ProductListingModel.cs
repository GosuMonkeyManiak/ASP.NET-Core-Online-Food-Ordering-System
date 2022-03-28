﻿namespace FoodFun.Web.Models.Product
{
    using System.ComponentModel.DataAnnotations;
    using Core.ValidationAttributes;
    
    using static Constants.GlobalConstants.Messages;

    public class ProductListingModel
    {
        [Required]
        [ShouldBeInActiveProductCategory]
        [ShouldBeExistingProduct]
        public string Id { get; init; }

        public string Name { get; init; }

        public string ImageUrl { get; init; }

        public string CategoryTitle { get; init; }
       
        public decimal Price { get; init; }
      
        public string Description { get; init; }

        [Range(
            1,
            long.MaxValue,
            ErrorMessage = ProductQuantityError)]
        public long Quantity { get; set; }
    }
}
