namespace FoodFun.Web.Controllers
{
    using Constants;
    using Core.Contracts;
    using Core.Models.Product;
    using Core.ValidationAttributes.Product;
    using Extensions;
    using global::AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Models.Cart;
    using Models.Product;
    
    using static Constants.GlobalConstants.Messages;

    public class CartController : Controller
    {
        private readonly IProductService productService;
        private readonly IMapper mapper;

        public CartController(
            IProductService productService, 
            IMapper mapper)
        {
            this.productService = productService;
            this.mapper = mapper;
        }

        [Authorize]
        public async Task<IActionResult> AddToCard([MustBeExistingProduct] [MustBeInActiveProductCategory] string id)
        {
            if (!this.ModelState.IsValid)
            {
                this.TempData[Error] = ProductNotExist;

                return RedirectToAction("All", "Product");
            }

            await this.HttpContext.Session.LoadAsync();

            var isCartExist = this.HttpContext.Session.Keys.Any(x => x == GlobalConstants.Cart);

            if (isCartExist)
            {
                var cartFromCache = this.HttpContext.Session.Get<CartModel>(GlobalConstants.Cart);
                var isProductExist = cartFromCache.Products.Any(x => x.Id == id);

                if (isProductExist)
                {
                    var productFromCache = cartFromCache.Products.First(x => x.Id == id);
                    productFromCache.Quantity++;
                }
                else
                {
                    var newProduct = new CartItemModel() { Id = id, Quantity = 1 };
                    cartFromCache.Products.Add(newProduct);
                }

                this.HttpContext.Session.Set<CartModel>(GlobalConstants.Cart, cartFromCache);
            }
            else
            {
                var newCart = new CartModel();
                newCart.Products.Add(new CartItemModel() { Id = id, Quantity = 1 });

                this.HttpContext.Session.Set<CartModel>(GlobalConstants.Cart, newCart);
            }

            await this.HttpContext.Session.CommitAsync();

            return RedirectToAction("All", "Product");
        }

        [Authorize]
        public async Task<IActionResult> Cart()
        {
            await this.HttpContext.Session.LoadAsync();

            if (!this.HttpContext.Session.IsAvailable
                || !this.HttpContext.Session.Keys.Any(x => x == GlobalConstants.Cart))
            {
                return View(new CartListingModel());
            }

            var cart = this.HttpContext.Session.Get<CartModel>(GlobalConstants.Cart);

            var productsFromService = await this.productService.All(cart.Products.Select(x => x.Id).ToArray());

            var products = MapToProductListingModels(productsFromService, cart.Products);

            return View(new CartListingModel()
            {
                Products = products
            });
        }

        [Authorize]
        public async Task<IActionResult> RemoveFromCart(string id)
        {
            await this.HttpContext.Session.LoadAsync();

            if (!this.HttpContext.Session.IsAvailable
                || !this.HttpContext.Session.Keys.Any(x => x == GlobalConstants.Cart))
            {
                return RedirectToAction(nameof(Cart));
            }

            var cart = this.HttpContext.Session.Get<CartModel>(GlobalConstants.Cart);

            if (!cart.Products.Any(x => x.Id == id))
            {
                return RedirectToAction(nameof(Cart));
            }

            var product = cart.Products.First(x => x.Id == id);
            cart.Products.Remove(product);

            this.HttpContext.Session.Set<CartModel>(GlobalConstants.Cart, cart);

            await this.HttpContext.Session.CommitAsync();

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
    }
}
