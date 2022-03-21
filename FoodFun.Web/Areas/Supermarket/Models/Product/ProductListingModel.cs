namespace FoodFun.Web.Areas.Supermarket.Models.Product
{
    public class ProductListingModel
    {
        public string Id { get; init; }

        public string Name { get; init; }

        public string ImageUrl { get; init; }

        public string CategoryTitle { get; init; }
       
        public decimal Price { get; init; }
      
        public string Description { get; init; }

        public long Quantity { get; init; }
    }
}
