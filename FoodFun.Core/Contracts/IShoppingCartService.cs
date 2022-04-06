namespace FoodFun.Core.Contracts
{
    using FoodFun.Core.Models.Cart;

    public interface IShoppingCartService
    {
        Task AddProductToCard(string productId, string cartKey);

        Task AddDishToCard(string dishId, string cartKey);

        CartModel GetCart(string cartKey);

        bool IsKeyExist(string key);

        bool IsCartAvailable();

        Task LoadCart();

        Task RemoveFromCart(string itemId, string cartKey);
    }
}
