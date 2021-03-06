namespace FoodFun.Web.Controllers
{
    using Constants;
    using Core.Contracts;
    using Core.Models.Product;
    using Core.ValidationAttributes.Product;
    using FoodFun.Core.Models.Cart;
    using FoodFun.Core.Models.Dish;
    using FoodFun.Core.ValidationAttributes.Dish;
    using FoodFun.Web.Areas.Restaurant.Models.Dish;
    using FoodFun.Web.Models.Cart;
    using global::AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Models.Product;

    using static Constants.GlobalConstants.Messages;

    public class CartController : Controller
    {
        private readonly IProductService productService;
        private readonly IDishService dishService;
        private readonly IShoppingCartService shoppingCartService;
        private readonly IMapper mapper;

        public CartController(
            IProductService productService,
            IDishService dishService,
            IShoppingCartService shoppingCartService,
            IMapper mapper)
        {
            this.productService = productService;
            this.dishService = dishService;
            this.shoppingCartService = shoppingCartService;
            this.mapper = mapper;
        }

        [Authorize]
        public async Task<IActionResult> AddProductToCard(
            [MustBeExistingProduct] 
            [MustBeInActiveProductCategory] string id)
        {
            if (!this.ModelState.IsValid)
            {
                this.TempData[Error] = ProductNotExist;

                return RedirectToAction("All", "Product");
            }

            await this.shoppingCartService.AddProductToCard(id, GlobalConstants.Cart);

            return RedirectToAction("All", "Product");
        }

        [Authorize]
        public async Task<IActionResult> AddDishToCard(
            [MustBeExistingDish]
            [MustBeInActiveDishCategory] string id)
        {
            if (!this.ModelState.IsValid)
            {
                this.TempData[Error] = DishNotExit;

                return RedirectToAction("All", "Dish");
            }

            await this.shoppingCartService.AddDishToCard(id, GlobalConstants.Cart);

            return RedirectToAction("All", "Dish");
        }

        [Authorize]
        public async Task<IActionResult> Cart()
        {
            await this.shoppingCartService.LoadCart();

            if (!this.shoppingCartService.IsCartAvailable()
                || !this.shoppingCartService.IsKeyExist(GlobalConstants.Cart))
            {
                return View(new CartListingModel());
            }

            var cart = this.shoppingCartService.GetCart(GlobalConstants.Cart);

            var productsFromService = await this.productService.All(cart.Products.Select(x => x.Id).ToArray());
            var dishesFromService = await this.dishService.All(cart.Dishes.Select(x => x.Id).ToArray());

            var products = MapToProductListingModels(productsFromService, cart.Products);
            var dishes = MapToDishListingModels(dishesFromService, cart.Dishes);

            return View(new CartListingModel()
            {
                Products = products,
                Dishes = dishes
            });
        }

        [Authorize]
        public async Task<IActionResult> RemoveFromCart(string id)
        {
            await this.shoppingCartService.LoadCart();

            if (!this.shoppingCartService.IsCartAvailable()
                || !this.shoppingCartService.IsKeyExist(GlobalConstants.Cart))
            {
                return RedirectToAction(nameof(Cart));
            }

            await this.shoppingCartService.RemoveFromCart(id, GlobalConstants.Cart);

            return RedirectToAction(nameof(Cart));
        }

        private IList<ProductListingModel> MapToProductListingModels(
            IEnumerable<ProductServiceModel> productsFromService,
            IList<CartItemModel> productsInCart)
        {
            var products = new List<ProductListingModel>();

            foreach (var productServiceModel in productsFromService)
            {
                var productListingModel = this.mapper.Map<ProductListingModel>(productServiceModel);
                productListingModel.Quantity =
                    productsInCart.FirstOrDefault(x => x.Id == productListingModel.Id).Quantity;

                products.Add(productListingModel);
            }

            return products;
        }

        private IList<DishListingModel> MapToDishListingModels(
            IEnumerable<DishServiceModel> dishesFromService,
            IList<CartItemModel> dishesInCart)
        {
            var dishes = new List<DishListingModel>();

            foreach (var dishServiceModel in dishesFromService)
            {
                var dishListingModel = this.mapper.Map<DishListingModel>(dishServiceModel);
                dishListingModel.Quantity =
                    dishesInCart.FirstOrDefault(x => x.Id == dishListingModel.Id).Quantity;

                dishes.Add(dishListingModel);
            }

            return dishes;
        }
    }
}
