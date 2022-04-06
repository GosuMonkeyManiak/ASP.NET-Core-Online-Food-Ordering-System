namespace FoodFun.Web.Models.Cart
{
    using FoodFun.Web.Areas.Restaurant.Models.Dish;
    using Product;

    public class CartListingModel
    {
        public IList<ProductListingModel> Products { get; set; } = new List<ProductListingModel>();

        public IList<DishListingModel> Dishes { get; set; } = new List<DishListingModel>();
    }
}
