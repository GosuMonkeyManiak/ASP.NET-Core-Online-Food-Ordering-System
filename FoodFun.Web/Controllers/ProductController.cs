namespace FoodFun.Web.Controllers
{
    using Core.Contracts;
    using Core.Extensions;
    using Extensions;
    using global::AutoMapper;
    using Infrastructure.Common;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Models.Cart;
    using Models.Product;

    using static Constants.GlobalConstants;
    using static Constants.GlobalConstants.Messages;

    public class ProductController : Controller
    {
        private readonly IProductService productService;
        private readonly IMapper mapper;
        private readonly IProductCategoryService productCategoryService;

        public ProductController(
            IProductService productService, 
            IMapper mapper, 
            IProductCategoryService productCategoryService)
        {
            this.productService = productService;
            this.mapper = mapper;
            this.productCategoryService = productCategoryService;
        }

        public async Task<IActionResult> All(ProductSearchModel searchModel)
        {
            var (filteredProducts,
                currentPageNumber,
                lastPageNumber,
                selectedCategoryId) = await this.productService
                .All(
                    searchModel.SearchTerm,
                    searchModel.CategoryId,
                    searchModel.OrderNumber,
                    searchModel.CurrentPageNumber,
                    DataConstants.PublicPageSize);

            var categoriesForProduct = await productCategoryService.AllNotDisabled();

            var productSearchModel = new ProductSearchModel()
            {
                CurrentPageNumber = currentPageNumber,
                LastPageNumber = lastPageNumber,
                SelectedCategoryId = selectedCategoryId,
                Products = filteredProducts.ProjectTo<ProductListingModel>(this.mapper),
                Categories = categoriesForProduct.ProjectTo<ProductCategoryModel>(this.mapper)
            };

            return View(productSearchModel);
        }

        [Authorize]
        public async Task<IActionResult> AddToCard(string id)
        {
            if (!await this.productService.IsProductExist(id))
            {
                this.TempData[Error] = ProductNotExist;

                return RedirectToAction(nameof(All));
            }

            await this.HttpContext.Session.LoadAsync();

            var isCartItemsExist = this.HttpContext.Session.Keys.Any(x => x == CartItems);

            if (isCartItemsExist)
            {
                var cartFromCache = this.HttpContext.Session.Get<Cart>(CartItems);
                var isProductExist = cartFromCache.Products.Any(x => x.Id == id);

                if (isProductExist)
                {
                    var productFromCache = cartFromCache.Products.First(x => x.Id == id);
                    productFromCache.Quantity++;
                }
                else
                {
                    var newProduct = new CartItem() { Id = id, Quantity = 1 };
                    cartFromCache.Products.Add(newProduct);
                }

                this.HttpContext.Session.Set<Cart>(CartItems, cartFromCache);
            }
            else
            {
                var newCart = new Cart();
                newCart.Products.Add(new CartItem(){ Id = id, Quantity = 1});

                this.HttpContext.Session.Set<Cart>(CartItems, newCart);
            }
            
            await this.HttpContext.Session.CommitAsync();

            return RedirectToAction(nameof(All));
        }
    }
}
