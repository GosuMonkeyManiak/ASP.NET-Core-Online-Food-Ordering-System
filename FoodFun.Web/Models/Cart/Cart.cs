namespace FoodFun.Web.Models.Cart
{
    public class Cart
    {
        public IList<CartItem> Products { get; set; } = new List<CartItem>();

        public IList<CartItem> Dishes { get; set; } = new List<CartItem>();
    }
}
