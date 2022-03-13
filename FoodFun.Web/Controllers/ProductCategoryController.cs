namespace FoodFun.Web.Controllers
{
    using Core.Contracts;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Models.ProductCategory;

    using static Constants.GlobalConstants.Roles;
    using static Constants.GlobalConstants.Messages;

    [Authorize(Roles = Administrator)]
    public class ProductCategoryController : Controller
    {
        private readonly IProductCategoryService productCategoryService;

        public ProductCategoryController(IProductCategoryService productCategoryService)
            => this.productCategoryService = productCategoryService;

        public async Task<IActionResult> Add()
            => View();

        [HttpPost]
        public async Task<IActionResult> Add(ProductCategoryFormModel formModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            await this.productCategoryService.Add(formModel.Title);

            return RedirectToAction(nameof(Add));
        }

        public async Task<IActionResult> All()
        {
            var productCategoriesFromService = await this.productCategoryService.All();

            var productCategories = productCategoriesFromService
                .Select(p => new ProductCategoryWithProductCountModel()
                {
                    Id = p.Id,
                    Title = p.Title,
                    //ProductsCount = p.ProductsCount
                })
                .ToList();

            return View(productCategories);
        }

        public async Task<IActionResult> Edit(int categoryId)
        {
            var (isSucceed, productCategoryServiceModel) = await this.productCategoryService
                .GetById(categoryId);

            if (!isSucceed)
            {
                TempData[nameof(ProductCategoryNotExist)] = ProductCategoryNotExist;

                return RedirectToAction(nameof(All));
            }

            var productCategoryModel = new ProductCategoryModel()
            {
                Id = productCategoryServiceModel.Id,
                Title = productCategoryServiceModel.Title
            };

            return View(productCategoryModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProductCategoryModel productCategoryModel)
        {
            if (!ModelState.IsValid)
            {
                return View(productCategoryModel);
            }

            var isSucceed = await this.productCategoryService
                .Update(
                    productCategoryModel.Id,
                    productCategoryModel.Title);

            if (!isSucceed)
            {
                TempData[nameof(ProductCategoryNotExist)] = ProductCategoryNotExist;
            }

            return RedirectToAction(nameof(All));
        }
    }
}
