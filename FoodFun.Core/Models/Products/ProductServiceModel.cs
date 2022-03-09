namespace FoodFun.Core.Models.Products
{
    public class ProductServiceModel
    {
        public string Id { get; init; }

        public string Name { get; init; }

        public string ImageUrl { get; init; }

        public ProductCategoryServiceModel Category { get; init; }

        public decimal Price { get; init; }

        public string Description { get; init; }
    }
}
