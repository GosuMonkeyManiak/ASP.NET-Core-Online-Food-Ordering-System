namespace FoodFun.Core.Services
{
    using FoodFun.Core.Contracts;
    using FoodFun.Core.Extensions;
    using FoodFun.Core.Models.Cart;
    using Microsoft.AspNetCore.Http;
    using System.Linq;

    public class ShoppingCartService : IShoppingCartService
    {
        private readonly HttpContext httpContext;

        public ShoppingCartService(IHttpContextAccessor httpContext) 
            => this.httpContext = httpContext.HttpContext;

        public async Task AddProductToCard(string productId, string cartKey)
        {
            await LoadCart();

            var isCartExist = IsKeyExist(cartKey);

            if (isCartExist)
            {
                var cartFromCache = GetCart(cartKey);
                var isProductExist = cartFromCache.Products.Any(x => x.Id == productId);

                if (isProductExist)
                {
                    var productFromCache = cartFromCache.Products.First(x => x.Id == productId);
                    productFromCache.Quantity++;
                }
                else
                {
                    var newProduct = new CartItemModel() { Id = productId, Quantity = 1 };
                    cartFromCache.Products.Add(newProduct);
                }

                SetCart(cartFromCache, cartKey);
            }
            else
            {
                var newCart = new CartModel();
                newCart.Products.Add(new CartItemModel() { Id = productId, Quantity = 1 });

                SetCart(newCart, cartKey);
            }

            await CommitAsync();
        }

        public async Task AddDishToCard(string dishId, string cartKey)
        {
            await LoadCart();

            var isCartExist = IsKeyExist(cartKey);

            if (isCartExist)
            {
                var cartFromCache = GetCart(cartKey);
                var isDishExist = cartFromCache.Dishes.Any(x => x.Id == dishId);

                if (isDishExist)
                {
                    var productFromCache = cartFromCache.Dishes.First(x => x.Id == dishId);
                    productFromCache.Quantity++;
                }
                else
                {
                    var newDish = new CartItemModel() { Id = dishId, Quantity = 1 };
                    cartFromCache.Dishes.Add(newDish);
                }

                SetCart(cartFromCache, cartKey);
            }
            else
            {
                var newCart = new CartModel();
                newCart.Dishes.Add(new CartItemModel() { Id = dishId, Quantity = 1 });

                SetCart(newCart, cartKey);
            }

            await CommitAsync();
        }

        public bool IsKeyExist(string key)
            => this.httpContext.Session.Keys.Any(x => x == key);

        public bool IsCartAvailable()
            => this.httpContext.Session.IsAvailable;

        public async Task LoadCart()
            => await this.httpContext.Session.LoadAsync();

        private async Task CommitAsync()
            => await this.httpContext.Session.CommitAsync();

        private void SetCart(CartModel cartModel, string cartKey)
            => this.httpContext.Session.Set<CartModel>(cartKey, cartModel);

        public CartModel GetCart(string cartKey)
            => this.httpContext.Session.Get<CartModel>(cartKey);
    }
}
