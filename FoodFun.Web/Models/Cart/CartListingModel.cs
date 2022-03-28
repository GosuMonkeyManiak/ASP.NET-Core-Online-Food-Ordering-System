namespace FoodFun.Web.Models.Cart
{
    using Product;

    public class CartListingModel
    {
        public IList<ProductListingModel> Products { get; set; } = new List<ProductListingModel>();
    }
}
