namespace FoodFun.Core.Models.Cart
{
    public class CartModel
    {
        public IList<CartItemModel> Products { get; set; } = new List<CartItemModel>();

        public IList<CartItemModel> Dishes { get; set; } = new List<CartItemModel>();
    }
}
