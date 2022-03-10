﻿namespace FoodFun.Core.Models.Product
{
    using ProductCategory;

    public class ProductServiceModel
    {
        public string Id { get; init; }

        public string Name { get; init; }

        public string ImageUrl { get; init; }

        public ProductCategoryWithProductCountServiceModel Category { get; init; }

        public decimal Price { get; init; }

        public string Description { get; init; }
    }
}
