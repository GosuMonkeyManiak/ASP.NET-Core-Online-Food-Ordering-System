namespace FoodFun.Web.Controllers
{
    using Core.Contracts;
    using Core.Extensions;
    using global::AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Models.Product;
    using Models.ProductCategory;

    using static Constants.GlobalConstants.Roles;
    using static Constants.GlobalConstants.Messages;
    
    public class ProductController : Controller
    {
        private readonly IProductService productService;
        private readonly IProductCategoryService productCategoryService;
        private readonly IMapper mapper;

        public ProductController(
            IProductService productService, 
            IProductCategoryService productCategoryService,
            IMapper mapper)
        {
            this.productService = productService;
            this.productCategoryService = productCategoryService;
            this.mapper = mapper;
        }

        [Authorize(Roles = Administrator)]
        public async Task<IActionResult> Add()
        {
            var categoriesForProduct = await this.productCategoryService.All();

            return View(new ProductFormModel()
            {
                Categories = categoriesForProduct.ProjectTo<ProductCategoryEditModel>(this.mapper)
            });
        }

        [Authorize(Roles = Administrator)]
        [HttpPost]
        public async Task<IActionResult> Add(ProductFormModel formModel)
        {
            if (!this.ModelState.IsValid)
            {
                return Add().GetAwaiter().GetResult();
            }
             
            var isSucceed = await this.productService
                .AddProduct(
                    formModel.Name,
                    formModel.ImageUrl,
                    formModel.CategoryId,
                    formModel.Price,
                    formModel.Description);

            if (!isSucceed)
            { 
                this.ModelState.AddModelError(string.Empty, ProductCategoryNotExist);

                return Add().GetAwaiter().GetResult();
            }

            return RedirectToAction(nameof(All));
        }

        [Authorize(Roles = $"{Administrator}, {Customer}")]
        public async Task<IActionResult> All([FromQuery] ProductSearchModel searchModel)
        {
            var productsWithCategories = await this.productService
                .All(
                    searchModel.SearchTerm,
                    searchModel.CategoryId,
                    searchModel.OrderNumber);

            var categoriesForProduct = await this.productCategoryService
                .All();

            var productSearchModel = new ProductSearchModel()
            {
                Products = productsWithCategories.ProjectTo<ProductListingModel>(this.mapper),
                Categories = categoriesForProduct.ProjectTo<ProductCategoryModel>(this.mapper)
            };

            return View(productSearchModel);
        }

        [Authorize(Roles = Administrator)]
        public async Task<IActionResult> Edit(string productId)
        {
            var (isSucceed, productServiceModel) = await this.productService
                .GetById(productId);

            if (!isSucceed)
            {
                this.TempData[nameof(Error)] = ProductNotExist;

                return RedirectToAction(nameof(All));
            }

            var productEditModel = this.mapper.Map<ProductEditModel>(productServiceModel);

            var categoriesForProduct = await this.productCategoryService
                .All();

            productEditModel.Categories = categoriesForProduct.ProjectTo<ProductCategoryEditModel>(this.mapper);

            return View(productEditModel);
        }

        [Authorize(Roles = Administrator)]
        [HttpPost]
        public async Task<IActionResult> Edit(ProductEditModel editModel)
        {
            if (!ModelState.IsValid)
            {
                return View(editModel);
            }

            var isSucceed = await this.productService
                .Update(
                    editModel.Id,
                    editModel.Name,
                    editModel.ImageUrl,
                    editModel.CategoryId,
                    editModel.Price,
                    editModel.Description);

            if (!isSucceed)
            {
                this.TempData[nameof(Error)] = ProductAndCategoryNotExist;
            }

            return RedirectToAction(nameof(All));
        }
    }
}
